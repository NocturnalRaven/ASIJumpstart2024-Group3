using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.WebApp.Models;

namespace ASI.Basecode.WebApp.Controllers;

public class UserController : Controller
{
  private readonly ILogger<UserController> _logger;

  public UserController(ILogger<UserController> logger)
  {
    _logger = logger;
  }

  #region HTTP GET

  [HttpGet]
  [Route("admin/dashboard/user")]
  public IActionResult Index()
  {
    return View("~/Views/Dashboard/User/Index.cshtml");
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
