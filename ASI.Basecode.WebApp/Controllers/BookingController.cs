using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    [Route("api/bookings")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(
            IBookingService bookingService,
            IUserService userService,
            IRoomService roomService,
            ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _userService = userService;
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet("GetAllBookings")]
        public JsonResult GetAllBookings()
        {
            try
            {
                var bookings = _bookingService.RetrieveAll();
                return Json(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all bookings: ", ex);
                return Json(new { error = "Failed to retrieve bookings" });
            }
        }

        [HttpGet("GetBookedRooms")]
        public JsonResult GetBookedRooms()
        {
            try
            {
                var bookedRooms = _bookingService.RetrieveBookedRooms();
                return Json(bookedRooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching booked rooms: ", ex);
                return Json(new { error = "Failed to retrieve booked rooms" });
            }
        }

        [HttpGet("GetBookedRoomById/{id}")]
        public JsonResult GetBookedRoomById(int id)
        {
            try
            {
                var bookedRoom = _bookingService.RetrieveBooking(id);
                if (bookedRoom == null)
                {
                    return Json(new { error = "Booked room not found" });
                }
                return Json(bookedRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching booked room with ID {id}: ", ex);
                return Json(new { error = "Failed to retrieve booked room" });
            }
        }

        [HttpPost("AddBooking")]
        public IActionResult AddBooking([FromBody] BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _bookingService.Add(model);
                    return Ok("Booking added successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error adding booking: ", ex);
                    return StatusCode(500, "Internal server error while adding booking");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Invalid booking data: {Errors}", string.Join(", ", errors));

            return BadRequest("Invalid booking data");
        }

        [HttpPut("UpdateBooking/{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingBooking = _bookingService.RetrieveBooking(id);
                    if (existingBooking == null)
                    {
                        return NotFound("Booking not found.");
                    }

                    model.BookingId = id; // Ensure the ID in the model matches the URL parameter
                    _bookingService.Update(model);
                    return Ok("Booking updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error updating booking: ", ex);
                    return StatusCode(500, "Internal server error while updating booking");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Invalid booking data: {Errors}", string.Join(", ", errors));

            return BadRequest("Invalid booking data");
        }

        [HttpDelete("DeleteBooking/{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                var booking = _bookingService.RetrieveBooking(id);
                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID {BookingId} not found for deletion", id);
                    return NotFound(new { message = "Booking not found." });
                }

                _bookingService.Delete(id);
                _logger.LogInformation("Booking with ID {BookingId} deleted successfully", id);
                return Ok(new { message = "Booking deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting booking with ID {BookingId}: {Exception}", id, ex);
                return StatusCode(500, new { error = "Internal server error while deleting booking." });
            }
        }


        [HttpDelete("DeleteBookedRoom/{id}")]
        public IActionResult DeleteBookedRoom(int id)
        {
            try
            {
                _bookingService.Delete(id);
                return Ok("Booking deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting booking: ", ex);
                return StatusCode(500, "Internal server error while deleting booking");
            }
        }

        [HttpPut("UpdateBookedRoom/{id}")]
        public IActionResult UpdateBookedRoom(int id, [FromBody] BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Model validation failed: {Errors}", string.Join(", ", errors));
                return BadRequest("Invalid booking data");
            }

            try
            {
                var existingBooking = _bookingService.RetrieveBooking(id);
                if (existingBooking == null)
                {
                    return NotFound("Booking not found.");
                }

                _bookingService.Update(model);
                return Ok("Booking updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating booking: ", ex);
                return StatusCode(500, "Internal server error while updating booking");
            }
        }


        [HttpGet("ViewBooking/{id}")]
        public JsonResult ViewBooking(int id)
        {
            try
            {
                var booking = _bookingService.RetrieveBooking(id);
                if (booking == null)
                {
                    return Json(new { error = "Booking not found" });
                }
                return Json(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching booking with ID {id}: ", ex);
                return Json(new { error = "Failed to retrieve booking" });
            }
        }

        // Additional endpoints
        [HttpGet("UserBookings")]
        public IActionResult UserBookings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid User ID");
            }

            var bookings = _bookingService.RetrieveAll(userId: userId) ?? new List<BookingViewModel>();
            return View(bookings);
        }

        [HttpGet("RecurringBookings")]
        public IActionResult RecurringBookings()
        {
            var recurringBookings = _bookingService.RetrieveAll(status: "Recurring");
            return View(recurringBookings);
        }

        [HttpGet("GetAllUsers")]
        public JsonResult GetAllUsers()
        {
            try
            {
                var users = _userService.RetrieveAll();
                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all users: ", ex);
                return Json(new { error = "Failed to retrieve users" });
            }
        }

        [HttpGet("GetAllRooms")]
        public JsonResult GetAllRooms()
        {
            try
            {
                var rooms = _roomService.GetAllRooms();
                return Json(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all rooms: ", ex);
                return Json(new { error = "Failed to retrieve rooms" });
            }
        }
    }
}
