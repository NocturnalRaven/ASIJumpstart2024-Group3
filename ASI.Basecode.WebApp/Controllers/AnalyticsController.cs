using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.WebApp.Models;

namespace ASI.Basecode.WebApp.Controllers;

public class AnalyticsController : Controller
{
  private readonly ILogger<AnalyticsController> _logger;

  public AnalyticsController(ILogger<AnalyticsController> logger)
  {
    _logger = logger;
  }

  #region HTTP GET

  [HttpGet]
  [Route("admin/dashboard/analytics")]
  public IActionResult Index()
  {
    return View("~/Views/Dashboard/Analytics/Index.cshtml");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  #endregion
}
