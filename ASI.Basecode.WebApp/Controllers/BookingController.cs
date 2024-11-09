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
    [ApiController]
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
        public IActionResult GetAllBookings()
        {
            try
            {
                var bookings = _bookingService.RetrieveAll();
                return Json(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all bookings: {Message}", ex.Message);
                return StatusCode(500, new { error = "Failed to retrieve bookings" });
            }
        }

        [HttpGet("GetBookedRooms")]
        public IActionResult GetBookedRooms()
        {
            try
            {
                var bookedRooms = _bookingService.RetrieveBookedRooms();
                return Json(bookedRooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching booked rooms: {Message}", ex.Message);
                return StatusCode(500, new { error = "Failed to retrieve booked rooms" });
            }
        }

        [HttpGet("GetBookedRoomById/{id}")]
        public IActionResult GetBookedRoomById(int id)
        {
            try
            {
                var bookedRoom = _bookingService.RetrieveBooking(id);
                if (bookedRoom == null)
                {
                    return NotFound(new { error = "Booked room not found" });
                }
                return Json(bookedRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching booked room with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, new { error = "Failed to retrieve booked room" });
            }
        }

        [HttpPost("AddBooking")]
        public IActionResult AddBooking([FromBody] BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Invalid booking data: {Errors}", string.Join(", ", errors));
                return BadRequest("Invalid booking data");
            }

            try
            {
                _bookingService.Add(model);
                return Ok(new { message = "Booking added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding booking: {Message}", ex.Message);
                return StatusCode(500, "Internal server error while adding booking");
            }
        }

        [HttpPut("UpdateBooking/{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Invalid booking data: {Errors}", string.Join(", ", errors));
                return BadRequest("Invalid booking data");
            }

            try
            {
                var existingBooking = _bookingService.RetrieveBooking(id);
                if (existingBooking == null)
                {
                    return NotFound(new { error = "Booking not found" });
                }

                model.BookingId = id;
                _bookingService.Update(model);
                return Ok(new { message = "Booking updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating booking with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, "Internal server error while updating booking");
            }
        }

        [HttpDelete("DeleteBooking/{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                var booking = _bookingService.RetrieveBooking(id);
                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID {Id} not found for deletion", id);
                    return NotFound(new { error = "Booking not found" });
                }

                _bookingService.Delete(id);
                return Ok(new { message = "Booking deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting booking with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, "Internal server error while deleting booking");
            }
        }

        [HttpDelete("DeleteBookedRoom/{id}")]
        public IActionResult DeleteBookedRoom(int id)
        {
            try
            {
                _bookingService.Delete(id);
                return Ok(new { message = "Booked room deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting booked room with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, "Internal server error while deleting booked room");
            }
        }

        [HttpPut("UpdateBookedRoom/{id}")]
        public IActionResult UpdateBookedRoom(int id, [FromBody] BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Invalid booking data: {Errors}", string.Join(", ", errors));
                return BadRequest("Invalid booking data");
            }

            try
            {
                var existingBooking = _bookingService.RetrieveBooking(id);
                if (existingBooking == null)
                {
                    return NotFound(new { error = "Booking not found" });
                }

                _bookingService.Update(model);
                return Ok(new { message = "Booking updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating booking with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, "Internal server error while updating booking");
            }
        }

        [HttpGet("ViewBooking/{id}")]
        public IActionResult ViewBooking(int id)
        {
            try
            {
                var booking = _bookingService.RetrieveBooking(id);
                if (booking == null)
                {
                    return NotFound(new { error = "Booking not found" });
                }
                return Json(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching booking with ID {Id}: {Message}", id, ex.Message);
                return StatusCode(500, "Failed to retrieve booking");
            }
        }

        [HttpGet("UserBookings")]
        public IActionResult UserBookings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid User ID");
            }

            try
            {
                var bookings = _bookingService.RetrieveAll(userId: userId) ?? new List<BookingViewModel>();
                return View(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching user bookings for User ID {UserId}: {Message}", userId, ex.Message);
                return StatusCode(500, "Failed to retrieve user bookings");
            }
        }

        [HttpGet("RecurringBookings")]
        public IActionResult RecurringBookings()
        {
            try
            {
                var recurringBookings = _bookingService.RetrieveAll(status: "Recurring");
                return View(recurringBookings);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching recurring bookings: {Message}", ex.Message);
                return StatusCode(500, "Failed to retrieve recurring bookings");
            }
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.RetrieveAll();
                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all users: {Message}", ex.Message);
                return StatusCode(500, "Failed to retrieve users");
            }
        }

        [HttpGet("GetAllRooms")]
        public IActionResult GetAllRooms()
        {
            try
            {
                var rooms = _roomService.GetAllRooms();
                return Json(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all rooms: {Message}", ex.Message);
                return StatusCode(500, "Failed to retrieve rooms");
            }
        }
    }
}
