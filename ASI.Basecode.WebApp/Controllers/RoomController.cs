using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.WebApp.Models;

namespace ASI.Basecode.WebApp.Controllers;

public class RoomController : Controller
{
  private readonly ILogger<RoomController> _logger;

  public RoomController(ILogger<RoomController> logger)
  {
    _logger = logger;
  }

  #region HTTP GET

  [HttpGet]
  [Route("admin/dashboard/room")]
  public IActionResult Index()
  {
    return View("~/Views/Dashboard/Room/Index.cshtml");
  }

  [Route("admin/dashboard/room/edit")]
  public IActionResult Edit()
  {
    return View("~/Views/Dashboard/Room/Edit.cshtml");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  #endregion
}
