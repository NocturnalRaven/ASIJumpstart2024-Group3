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
                return BadRequest(new { message = "Invalid booking data", errors });
            }

            try
            {
                if (model.IsRecurring == true && !string.IsNullOrEmpty(model.Frequency) && model.RecurringEndDate.HasValue)
                {
                    // Ensure RecurringEndDate is valid
                    if (model.RecurringEndDate <= model.StartDate)
                    {
                        _logger.LogWarning("Recurring end date must be after the start date");
                        return BadRequest(new { message = "Recurring end date must be after the start date" });
                    }

                    // Generate recurring bookings using the service method
                    var recurringBookings = _bookingService.GenerateRecurringBookings(model);

                    if (!recurringBookings.Any())
                    {
                        _logger.LogWarning("No recurring bookings generated for the provided frequency and end date");
                        return BadRequest(new { message = "No recurring bookings generated. Please check the frequency and end date." });
                    }

                    // Add each recurring booking
                    foreach (var recurringBooking in recurringBookings)
                    {
                        _bookingService.Add(recurringBooking);
                    }

                    _logger.LogInformation("Successfully added {Count} recurring bookings", recurringBookings.Count());
                }
                else
                {
                    // Add single booking
                    _bookingService.Add(model);
                    _logger.LogInformation("Successfully added a single booking with ID {BookingId}", model.BookingId);
                }

                return Ok(new { message = "Booking(s) added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding booking");
                return StatusCode(500, new { message = "Internal server error while adding booking", error = ex.Message });
            }
        }


        private IEnumerable<BookingViewModel> GenerateRecurringBookings(BookingViewModel model)
        {
            var recurringBookings = new List<BookingViewModel>();

            // Since model.StartDate and model.EndDate are non-nullable, use them directly
            DateTime nextStartDate = model.StartDate;
            DateTime nextEndDate = model.EndDate;

            // Define an end date for recurrence (e.g., one year from the start date)
            DateTime recurrenceEndDate = model.StartDate.AddYears(1);

            while (nextStartDate <= recurrenceEndDate)
            {
                var newBooking = new BookingViewModel
                {
                    UserId = model.UserId,
                    RoomId = model.RoomId,
                    StartDate = nextStartDate,
                    EndDate = nextEndDate,
                    NoOfPeople = model.NoOfPeople,
                    Status = model.Status,
                    IsRecurring = true,
                    Frequency = model.Frequency
                };

                recurringBookings.Add(newBooking);

                // Move to the next recurrence based on frequency
                switch (model.Frequency.ToLower())
                {
                    case "daily":
                        nextStartDate = nextStartDate.AddDays(1);
                        nextEndDate = nextEndDate.AddDays(1);
                        break;
                    case "weekly":
                        nextStartDate = nextStartDate.AddDays(7);
                        nextEndDate = nextEndDate.AddDays(7);
                        break;
                    case "monthly":
                        nextStartDate = nextStartDate.AddMonths(1);
                        nextEndDate = nextEndDate.AddMonths(1);
                        break;
                    default:
                        throw new ArgumentException("Invalid frequency specified.");
                }
            }

            return recurringBookings;
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

        [HttpPut("ArchiveExpiredBookings")]
        public IActionResult ArchiveExpiredBookings()
        {
            try
            {
                var archivedCount = _bookingService.ArchiveExpiredBookings();
                return Ok(new { message = $"{archivedCount} expired bookings archived successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error archiving expired bookings: {Message}", ex.Message);
                return StatusCode(500, "Failed to archive expired bookings");
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
                return Ok(bookings); // Return as JSON
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching user bookings for User ID {UserId}: {Message}", userId, ex.Message);
                return StatusCode(500, "Failed to retrieve user bookings");
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
