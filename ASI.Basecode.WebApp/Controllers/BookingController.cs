using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult UserBookings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                // Handle the case where the user is not authenticated
                return Unauthorized();
            }

            // Try parsing userId
            if (!int.TryParse(userIdString, out int userId))
            {
                // Handle invalid UserId
                return BadRequest("Invalid User ID");
            }

            // Fetch bookings for the user
            var bookings = _bookingService.RetrieveAll(userId: userId);

            // Ensure bookings is not null
            if (bookings == null)
            {
                bookings = new List<BookingViewModel>();
            }

            return View(bookings); // Pass the bookings to the view
        }


        public IActionResult RecurringBookings()
        {
            // Retrieve recurring bookings (you can filter recurring ones in RetrieveAll method)
            var recurringBookings = _bookingService.RetrieveAll(/* apply filters for recurring bookings */);

            return View(recurringBookings);
        }

        // Example for adding a booking
        [HttpPost]
        public IActionResult AddBooking(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _bookingService.Add(model);
                return RedirectToAction("UserBookings");
            }
            return View(model);
        }
    }
}
