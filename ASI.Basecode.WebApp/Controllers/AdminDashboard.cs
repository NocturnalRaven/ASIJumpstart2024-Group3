using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRoomService _roomService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>

        public AdminDashboard(IUserService userService, IRoomService roomService,
           IHttpContextAccessor httpContextAccessor,
                             ILoggerFactory loggerFactory,
                             IConfiguration configuration,
                             IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userService = userService;
            _roomService = roomService;
        }


        #region GET Methods

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin/dashboard")]
        public IActionResult Admindashboard()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin/dashboard/users")]
        public IActionResult AdminUserDashboard()
        {
            try
            {
                _logger.LogInformation("=======Retrieve All Start=======");
                var data = _userService.RetrieveAll().ToList();
                var role = UserRole;
                ViewData["Role"] = role;

                // Pass the correct model type
                var model = new UserPageViewModel
                {
                    UserList = new UserListViewModel
                    {
                        dataList = data
                    },
                    NewUser = new UserViewModel()
                };

                _logger.LogInformation("=======Retrieve All End==========");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(null);
            }
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin/dashboard/rooms")]
        public IActionResult Rooms()
        {
            try
            {
                _logger.LogInformation("=======Retrieve All Start=======");
                var data = _roomService.RetrieveAll().ToList();
                var role = UserRole;
                ViewData["Role"] = role;

                // Pass the correct model type
                var model = new RoomPageViewModel
                {
                    RoomList = new RoomListViewModel
                    {
                        dataList = data
                    },
                    NewRoom = new RoomViewModel()
                };

                _logger.LogInformation("=======Retrieve All End==========");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(null);
            }
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
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
            _logger.LogInformation("=======User Creation: PostCreate Start=======");

            try
            {
                // Check for duplicate UserCode
                bool isExist = _userService.RetrieveAll().Any(data => data.UserCode == model.UserCode);
                if (isExist)
                {
                    TempData["DuplicateErr"] = "Duplicate UserCode: " + model.UserCode;
                    _logger.LogError($"Duplicate UserCode: {model.UserCode}");
                    return RedirectToAction("AdminUserDashboard");
                }

                // Add the user using the UserService
                _userService.Add(model);
                TempData["CreateMessage"] = "User added successfully!";
                return RedirectToAction("AdminUserDashboard");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message; // Handle validation errors (e.g. empty UserCode)
                return RedirectToAction("AdminUserDashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                TempData["ErrorMessage"] = "An error occurred while adding the user.";
                return RedirectToAction("AdminUserDashboard");
            }
        }


        [HttpPost]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult PostUpdate(UserViewModel model)
        {
            _userService.Update(model);
            return RedirectToAction("AdminUserDashboard");
        }

        [HttpPost]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult PostDelete(int Id)
        {
            try
            {
                _userService.Delete(Id);
                _logger.LogInformation($"User with Id {Id} deleted successfully.");

                return RedirectToAction("AdminUserDashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with Id {Id}: {ex.Message}");

                return RedirectToAction("AdminUserDashboard");
            }
        }

        [HttpPost]
        public IActionResult PostCreateRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Or return the appropriate view with the model to re-render the form
            }

            _roomService.Add(model);
            TempData["CreateMessage"] = "Room added successfully!";
            return RedirectToAction("Rooms");
        }

        #endregion
    }
}
