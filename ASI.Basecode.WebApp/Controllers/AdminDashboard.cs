using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminDashboard : ControllerBase<AdminDashboard>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor for AdminDashboard
        /// </summary>
        public AdminDashboard(IUserService userService,
           IHttpContextAccessor httpContextAccessor,
           ILoggerFactory loggerFactory,
           IConfiguration configuration,
           IMapper mapper = null)
           : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userService = userService;
        }

        #region GET Methods

        [HttpGet]
        [Route("/admin/dashboard")]
        public IActionResult Admindashboard()
        {
            return View();
        }

        [HttpGet]
        [Route("/admin/dashboard/users")]
        public IActionResult AdminUserDashboard()
        {
            try
            {
                _logger.LogInformation("Start retrieving all users for Admin User Dashboard.");
                var data = _userService.RetrieveAll().Where(data => data.Role != 9).ToList();
                ViewData["Role"] = UserRole;

                var model = new UserPageViewModel
                {
                    UserList = new UserListViewModel { dataList = data },
                    NewUser = new UserViewModel()
                };

                _logger.LogInformation("Successfully retrieved all users for Admin User Dashboard.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving users: {Message}", ex.Message);
                return View(null);
            }
        }

        [HttpGet]
        [Route("/admin-dashboard/rooms")]
        public IActionResult Rooms()
        {
            return View();
        }

        [HttpGet]
        [Route("/admin/dashboard/analytics")]
        public IActionResult AdminAnalytics()
        {
            return View();
        }

        #endregion

        #region POST Methods

        [HttpPost]
        public IActionResult PostCreate(UserViewModel model)
        {
            _logger.LogInformation("Start user creation.");

            try
            {
                if (_userService.RetrieveAll().Any(data => data.UserCode == model.UserCode))
                {
                    TempData["DuplicateErr"] = $"Duplicate UserCode: {model.UserCode}";
                    _logger.LogWarning("Attempted to create a duplicate UserCode: {UserCode}", model.UserCode);
                    return RedirectToAction("AdminUserDashboard");
                }

                _userService.Add(model);
                TempData["CreateMessage"] = "User added successfully!";
                _logger.LogInformation("User created successfully.");
                return RedirectToAction("AdminUserDashboard");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError("User creation failed due to invalid input: {Message}", ex.Message);
                return RedirectToAction("AdminUserDashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding user: {Message}", ex.Message);
                TempData["ErrorMessage"] = "An error occurred while adding the user.";
                return RedirectToAction("AdminUserDashboard");
            }
        }

        [HttpPost]
        public IActionResult PostUpdate(UserViewModel model)
        {
            try
            {
                _userService.Update(model);
                TempData["UpdateMessage"] = "User updated successfully!";
                _logger.LogInformation("User with Id {Id} updated successfully.", model.Id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating user.";
                _logger.LogError("Error updating user with Id {Id}: {Message}", model.Id, ex.Message);
            }
            return RedirectToAction("AdminUserDashboard");
        }

        [HttpPost]
        public IActionResult PostDelete(int id)
        {
            try
            {
                _userService.Delete(id);
                TempData["DeleteMessage"] = $"User with Id {id} deleted successfully.";
                _logger.LogInformation("User with Id {Id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting user with Id {id}.";
                _logger.LogError("Error deleting user with Id {Id}: {Message}", id, ex.Message);
            }
            return RedirectToAction("AdminUserDashboard");
        }

        #endregion
    }
}
