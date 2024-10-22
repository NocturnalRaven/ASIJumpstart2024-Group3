﻿using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = repository;
        }

        public IEnumerable<UserViewModel> RetrieveAll(int? id = null, string firstName = null)
        {
            var data = _userRepository.GetUsers()
                .Where(x => x.Deleted != true
                        && (!id.HasValue || x.UserId == id)
                        && (string.IsNullOrEmpty(firstName) || x.FirstName.Contains(firstName)))
                .Select(s => new UserViewModel
                {
                    Id = s.UserId,
                    Name = string.Concat(s.FirstName, " ", s.LastName),
                    Email = s.Mail,
                    Role = s.UserRole == 9 ? "Super Admin"
                        : s.UserRole == 1 ? "Admin"
                        : s.UserRole == 2 ? "Staff"
                        : s.UserRole == 3 ? "User"
                        : "Unknown",
                    DateCreated = s.InsDt,
                });
            return data;
        }

        public UserViewModel RetrieveUser(int id)
        {
            var data = _userRepository.GetUsers().FirstOrDefault(x => x.Deleted != true && x.UserId == id);
            var model = new UserViewModel
            {
                Id = data.UserId,
                UserCode = data.UserCode,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Password = PasswordManager.DecryptPassword(data.Password)
            };
            return model;
        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Add(UserViewModel model)
        {
            var newModel = new MUser();
            newModel.UserCode = model.UserCode;
            newModel.FirstName = model.FirstName;
            newModel.LastName = model.LastName;
            newModel.Password = PasswordManager.EncryptPassword(model.Password);
            newModel.UserRole = 1;

            _userRepository.AddUser(newModel);
        }

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Update(UserViewModel model)
        {
            var existingData = _userRepository.GetUsers().Where(s => s.Deleted != true && s.UserId == model.Id).FirstOrDefault();
            existingData.UserCode = model.UserCode;
            existingData.FirstName = model.FirstName;
            existingData.LastName = model.LastName;
            existingData.Password = PasswordManager.EncryptPassword(model.Password);

            _userRepository.UpdateUser(existingData);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                user.Deleted = true;
                _userRepository.UpdateUser(user);  // Mark user as deleted
            }
        }

        public LoginResult AuthenticateUser(string userCode, string password, ref MUser user)
        {
            user = new MUser();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _userRepository.GetUsers().Where(x => x.UserCode == userCode &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }
    }
}
