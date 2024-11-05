using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
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
    /// <summary>
    /// User Management Controller
    /// </summary>
    public class UserController : ControllerBase<UserController>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserController(IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns the main user management view.
        /// </summary>
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Fetching users for the dashboard.");
                var data = _userService.RetrieveAll();
                var model = new UserListViewModel
                {
                    dataList = data.ToList() // Convert to a List to avoid null references
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching users: ", ex);
                return View(new UserListViewModel()); // Return an empty model to avoid null references
            }
        }

        /// <summary>
        /// Searches for users based on filter criteria.
        /// </summary>
        [HttpPost]
        public IActionResult SearchUsers(UserListViewModel model)
        {
            try
            {
                _logger.LogInformation("Searching users");
                var users = _userService.RetrieveAll(
                    string.IsNullOrEmpty(model.IdFilter) ? null : (int?)int.Parse(model.IdFilter),
                    model.FirstNameFilter
                ).ToList();

                var viewModel = new UserListViewModel { dataList = users };
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users");
                return View("Index", new UserListViewModel());
            }
        }

        #region GET Methods

        /// <summary>
        /// Returns the user creation view.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Returns user details for the specified ID.
        /// </summary>
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _userService.RetrieveUser(id);
            if (user == null) return NotFound();
            return View(user);
        }

        /// <summary>
        /// Returns the user edit view for the specified ID.
        /// </summary>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userService.RetrieveUser(id);
            if (user == null) return NotFound();
            return View(user);
        }

        /// <summary>
        /// Returns the delete confirmation view for the specified ID.
        /// </summary>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _userService.RetrieveUser(id);
            if (user == null) return NotFound();
            return View(user);
        }
        #endregion

        #region POST Methods

        /// <summary>
        /// Handles user creation.
        /// </summary>
        [HttpPost]
        public IActionResult PostCreate(UserViewModel model)
        {
            _logger.LogInformation("Creating new user");
            if (ModelState.IsValid)
            {
                try
                {
                    bool isExist = _userService.RetrieveAll().Any(u => u.UserCode == model.UserCode);
                    if (isExist)
                    {
                        TempData["DuplicateErr"] = "Duplicate User Code";
                        return RedirectToAction("Create");
                    }

                    _userService.Add(model);
                    TempData["CreateMessage"] = "User added successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding user");
                }
            }

            TempData["ErrorMessage"] = "Invalid data";
            return RedirectToAction("Create");
        }

        /// <summary>
        /// Handles user updates.
        /// </summary>
        [HttpPost]
        public IActionResult PostUpdate(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.Update(model);
                    TempData["UpdateMessage"] = "User updated successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user");
                }
            }

            TempData["ErrorMessage"] = "Invalid data";
            return RedirectToAction("Edit", new { id = model.Id });
        }

        /// <summary>
        /// Handles user deletion.
        /// </summary>
        [HttpPost]
        public IActionResult PostDelete(int id)
        {
            try
            {
                _userService.Delete(id);
                TempData["DeleteMessage"] = "User deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                TempData["ErrorMessage"] = "Error deleting user";
                return RedirectToAction("Delete", new { id });
            }
        }

        #endregion

        #region API Methods

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                _logger.LogError("Validation errors: " + string.Join("; ", errors));
                return BadRequest("Invalid user data: " + string.Join("; ", errors));
            }

            try
            {
                _userService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user via API");
                return StatusCode(500, "Internal server error while updating user");
            }
        }


        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user via API");
                return StatusCode(500, "Internal server error while deleting user");
            }
        }

        #endregion
    }
}
