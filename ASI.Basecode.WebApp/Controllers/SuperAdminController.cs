using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class SuperAdminController : Controller
    {
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
            return View();
        }
    }
}
