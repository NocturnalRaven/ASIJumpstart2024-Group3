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
                    return RedirectToAction("Accounts");
                }

                _userService.Add(model);
                TempData["CreateMessage"] = "User added successfully!";
                _logger.LogInformation("User created successfully.");
                return RedirectToAction("Accounts");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError("User creation failed due to invalid input: {Message}", ex.Message);
                return RedirectToAction("Accounts");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while adding user: {Message}", ex.Message);
                TempData["ErrorMessage"] = "An error occurred while adding the user.";
                return RedirectToAction("Accounts");
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
            return RedirectToAction("Accounts");
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
            return RedirectToAction("Accounts");
        }
    }
}
