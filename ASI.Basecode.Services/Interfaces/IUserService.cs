using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users with optional filters.
        /// </summary>
        /// <param name="id">Optional user ID filter.</param>
        /// <param name="firstName">Optional first name filter.</param>
        /// <returns>A collection of UserViewModel.</returns>
        IEnumerable<UserViewModel> RetrieveAll(int? id = null, string firstName = null);

        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>UserViewModel containing user details.</returns>
        UserViewModel RetrieveUser(int id);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="model">UserViewModel containing user data.</param>
        void Add(UserViewModel model);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="model">UserViewModel containing updated user data.</param>
        void Update(UserViewModel model);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        void Delete(int id);

        /// <summary>
        /// Authenticates a user with provided credentials.
        /// </summary>
        /// <param name="userCode">User code for login.</param>
        /// <param name="password">Password for login.</param>
        /// <param name="user">Reference to the MUser object for returning user details.</param>
        /// <returns>LoginResult indicating the outcome of authentication.</returns>
        LoginResult AuthenticateUser(string userCode, string password, ref MUser user);

        /// <summary>
        /// Optional: Add user role management methods if needed.
        /// </summary>
        // void AddUser(UserViewModel model);
    }
}
