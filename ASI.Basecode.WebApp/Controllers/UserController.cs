using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers;

public class UserController : ControllerBase<UserController>
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
    public UserController(IUserService userService,
        IHttpContextAccessor httpContextAccessor,
                          ILoggerFactory loggerFactory,
                          IConfiguration configuration,
                          IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
    {
        _userService = userService;
    }

    #region HTTP GET

    [HttpGet]
    [Route("admin/dashboard/user")]
    public IActionResult Index()
    {
        try
        {
            _logger.LogInformation("=======Sample Crud : Retrieve All Start=======");

            var data = _userService.RetrieveAll();
            var role = UserRole;
            ViewData["Role"] = role;

            // Create an instance of UserPageViewModel
            var model = new UserPageViewModel
            {
                UserList = new UserListViewModel
                {
                    dataList = data
                }
            };
            _logger.LogInformation("=======Sample Crud : Retrieve All End=======");
            return View("~/Views/Dashboard/User/Index.cshtml", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return View("~/Views/Dashboard/User/Index.cshtml");
        }
    }

    [Route("admin/dashboard/user/edit")]
    public IActionResult Edit()
    {
        return View("~/Views/Dashboard/User/Edit.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #endregion
}
