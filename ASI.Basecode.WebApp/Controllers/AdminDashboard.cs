using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminDashboard : Controller
    {
        public IActionResult Admindashboard()
        {
            return View();
        }

        public IActionResult Rooms()
        {
            return View();
        }


        public IActionResult AdminUserDashboard()
        {
            return View();
        }

        public IActionResult AdminAnalytics()
        {

            return View();
        }
    }
}
