using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.WebApp.Models;

namespace ASI.Basecode.WebApp.Controllers;

public class ProfileController : Controller
{
  private readonly ILogger<ProfileController> _logger;

  public ProfileController(ILogger<ProfileController> logger)
  {
    _logger = logger;
  }

  #region HTTP GET

  [HttpGet]
  [Route("admin/dashboard/profile")]
  public IActionResult Index()
  {
    return View("~/Views/Dashboard/Profile/Index.cshtml");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  #endregion
}
