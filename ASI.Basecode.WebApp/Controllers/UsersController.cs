using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult UserDashboard()
        {
            return View();
        }

        public IActionResult UserBookings() 
        { return View(); 
        }

        public IActionResult UserAnalytics()
        {
            return View();
        }
    }
}
