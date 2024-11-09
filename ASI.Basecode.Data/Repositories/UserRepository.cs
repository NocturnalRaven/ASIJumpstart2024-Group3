using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        public IQueryable<MUser> GetUsers()
        {
            return GetDbSet<MUser>().Where(u => !u.Deleted);
        }

        /// <summary>
        /// Checks if a user exists by ID.
        /// </summary>
        public bool UserExists(int userId)
        {
            return GetDbSet<MUser>().Any(x => x.UserId == userId && !x.Deleted);
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        public void AddUser(MUser user)
        {
            user.UserId = GetDbSet<MUser>().Max(x => (int?)x.UserId) + 1 ?? 1; // Handles case where DB is empty
            user.InsDt = DateTime.Now;
            user.UpdDt = DateTime.Now;
            GetDbSet<MUser>().Add(user);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        public void UpdateUser(MUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UpdDt = DateTime.Now;
            GetDbSet<MUser>().Update(user);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Marks a user as deleted.
        /// </summary>
        public void DeleteUser(int userId)
        {
            var userToDelete = GetDbSet<MUser>().FirstOrDefault(x => x.UserId == userId && !x.Deleted);
            if (userToDelete != null)
            {
                userToDelete.Deleted = true;
                userToDelete.UpdDt = DateTime.Now;
                UnitOfWork.SaveChanges();
            }
        }
    }
}
