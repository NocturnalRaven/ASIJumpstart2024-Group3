using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<SuperAdminController> _logger;
        private readonly IMapper _mapper;
        private readonly string _userRole;

        public SuperAdminController(IUserService userService,
                                    IHttpContextAccessor httpContextAccessor,
                                    ILogger<SuperAdminController> logger,
                                    IConfiguration configuration,
                                    IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _userRole = httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? "User";
        }

        public IActionResult Rooms()
        {
            return View();
        }

        public IActionResult Bookings()
        {
            return View();
        }

        public IActionResult Accounts()
        {
            try
            {
                _logger.LogInformation("Start retrieving all users for Admin User Dashboard.");
                var data = _userService.RetrieveAll().ToList();

                ViewData["Role"] = _userRole;

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
    }
}
