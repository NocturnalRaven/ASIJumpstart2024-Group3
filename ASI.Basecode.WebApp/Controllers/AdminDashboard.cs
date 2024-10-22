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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>

        public AdminDashboard(IUserService userService,
           IHttpContextAccessor httpContextAccessor,
                             ILoggerFactory loggerFactory,
                             IConfiguration configuration,
                             IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userService = userService;
        }


        #region GET Methods

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/index")]
        public IActionResult Admindashboard()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/users")]
        public IActionResult AdminUserDashboard()
        {
            try
            {
                _logger.LogInformation("=======Retrieve All Start=======");
                var data = _userService.RetrieveAll();
                var role = UserRole;
                ViewData["Role"] = role;

                // Pass the correct model type
                var model = new UserPageViewModel
                {
                    UserList = new UserListViewModel
                    {
                        dataList = data
                    },
                    NewUser = new UserViewModel() // Initialize the NewUser if needed
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
        [Route("/admin-dashboard/rooms")]
        public IActionResult Rooms()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/analytics")]
        public IActionResult AdminAnalytics()
        {

            return View();
        }

        #endregion

        #region POST Methods
        [HttpPost]
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
        #endregion
    }
}
