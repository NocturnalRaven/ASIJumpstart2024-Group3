using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
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
            _userRepository = repository;
            _mapper = mapper;
        }

        public IEnumerable<UserViewModel> RetrieveAll(int? id = null, string firstName = null)
        {
            return _userRepository.GetUsers()
                .Where(x => !x.Deleted &&
                            (!id.HasValue || x.UserId == id.Value) &&
                            (string.IsNullOrEmpty(firstName) || x.FirstName.Contains(firstName)))
                .Select(s => new UserViewModel
                {
                    Id = s.UserId,
                    Name = $"{s.FirstName} {s.LastName}",
                    Email = s.Mail,
                    Role = s.UserRole ?? 0,
                    DateCreated = s.InsDt,
                    DateModified = s.UpdDt
                })
                .ToList();
        }

        public UserViewModel RetrieveUser(int id)
        {
            var data = _userRepository.GetUsers().FirstOrDefault(x => !x.Deleted && x.UserId == id);
            if (data == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return new UserViewModel
            {
                Id = data.UserId,
                UserCode = data.UserCode,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Mail,
                Role = data.UserRole ?? 0,
                // Avoid exposing decrypted passwords; consider adding placeholder or omitting
                Password = "********"
            };
        }

        public void Add(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserCode))
            {
                throw new ArgumentException("UserCode cannot be null or empty.");
            }

            var newModel = new MUser
            {
                UserCode = model.UserCode,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mail = model.Email,
                Password = PasswordManager.EncryptPassword(model.Password),
                UserRole = model.Role,
                InsDt = DateTime.Now,
                UpdDt = DateTime.Now
            };

            _userRepository.AddUser(newModel);
        }

        public void Update(UserViewModel model)
        {
            var existingData = _userRepository.GetUsers().FirstOrDefault(s => !s.Deleted && s.UserId == model.Id);
            if (existingData == null)
            {
                throw new KeyNotFoundException($"User with ID {model.Id} not found for update.");
            }

            existingData.UserCode = model.UserCode;
            existingData.FirstName = model.FirstName;
            existingData.LastName = model.LastName;
            existingData.Mail = model.Email;
            existingData.UserRole = model.Role;
            existingData.Password = PasswordManager.EncryptPassword(model.Password);
            existingData.UpdDt = DateTime.Now;

            _userRepository.UpdateUser(existingData);
        }

        public void Delete(int id)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(u => u.UserId == id && !u.Deleted);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found for deletion.");
            }

            _userRepository.DeleteUser(id);
        }

        public LoginResult AuthenticateUser(string userCode, string password, ref MUser user)
        {
            var encryptedPassword = PasswordManager.EncryptPassword(password);
            user = _userRepository.GetUsers()
                .FirstOrDefault(x => x.UserCode == userCode && x.Password == encryptedPassword);

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }
    }
}
