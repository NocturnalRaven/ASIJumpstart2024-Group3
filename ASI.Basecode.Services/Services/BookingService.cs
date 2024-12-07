using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository repository, IMapper mapper, INotificationService notificationService, IUserRepository userRepository, ILogger<BookingService> logger)
        {
            _mapper = mapper;
            _bookingRepository = repository;
            _notificationService = notificationService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public IEnumerable<BookingViewModel> RetrieveAll(int? bookingId = null, int? userId = null, DateTime? startDate = null, string status = null)
        {
            var data = _bookingRepository.GetBooking()
                .Where(x => !x.Deleted
                        && (!bookingId.HasValue || x.Id == bookingId)
                        && (!userId.HasValue || x.UserId == userId)
                        && (!startDate.HasValue || x.StartDate == startDate)
                        && (string.IsNullOrEmpty(status) || x.Status.Contains(status)))
                .Select(s => _mapper.Map<BookingViewModel>(s));

            return data;
        }

        public BookingViewModel RetrieveBooking(int id)
        {
            var booking = _bookingRepository.GetBooking().FirstOrDefault(x => !x.Deleted && x.Id == id);
            return booking != null ? _mapper.Map<BookingViewModel>(booking) : null;
        }

        public void Add(BookingViewModel model)
        {
            var newBooking = _mapper.Map<Booking>(model);
            newBooking.CreatedAt = DateTime.Now;
            newBooking.UpdatedAt = DateTime.Now;

            _bookingRepository.AddBooking(newBooking);

            try 
            {
                var subject = "Booking Confirmation";
                var message = $@"
                    Dear User,
                    
                    Your booking for Room ID {model.RoomId} has been successfully confirmed.
                    Start Date: {model.StartDate}
                    End Date: {model.EndDate}

                    Thank you for booking with us!
                ";
                var user = _userRepository.GetUserById(newBooking.UserId);
                _notificationService.SendEmail(user.Email, subject, message);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while sending booking confirmation email.");
            }
        }

        public void Update(BookingViewModel model)
        {
            var existingBooking = _bookingRepository.GetBooking().FirstOrDefault(s => !s.Deleted && s.Id == model.BookingId);

            if (existingBooking != null)
            {
                existingBooking.UserId = model.UserId;
                existingBooking.RoomId = model.RoomId;
                existingBooking.StartDate = model.StartDate;
                existingBooking.EndDate = model.EndDate;
                existingBooking.NoOfPeople = model.NoOfPeople;
                existingBooking.Status = model.Status;
                existingBooking.IsRecurring = model.IsRecurring;
                existingBooking.Frequency = model.Frequency;
                existingBooking.UpdatedAt = DateTime.Now;

                _bookingRepository.UpdateBooking(existingBooking);
            }
            else
            {
                throw new InvalidOperationException("Booking not found.");
            }
        }

        public void Delete(int id)
        {
            _bookingRepository.DeleteBooking(id);
        }

        public void SendBookingReminders()
        {
            var upcomingBookings = _bookingRepository.GetBooking()
                .Where(b => !b.Deleted && b.StartDate == DateTime.Now.AddDays(2).Date)
                .ToList();

            foreach (var booking in upcomingBookings)
            {
                try
                {
                    var user = _userRepository.GetUsers().FirstOrDefault(u => u.UserId == booking.UserId);
                    var subject = "Booking Reminder";
                    var message = $@"
                        Dear {user.FirstName},
                
                        This is a reminder for your upcoming booking:
                        Room ID: {booking.RoomId}
                        Start Date: {booking.StartDate}
                        End Date: {booking.EndDate}
                
                        We look forward to hosting you!
                    ";

                    _notificationService.SendEmail(user.Email, subject, message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while sending reminder email for booking ID {BookingId}", booking.Id);
                }
            }
        }

        public IEnumerable<BookedRoomViewModel> RetrieveBookedRooms()
        {
            var data = _bookingRepository.GetBooking()
                .Where(x => !x.Deleted && x.Room.Status == "Booked")
                .Include(b => b.Room)
                .Include(b => b.User)
                .Select(s => new BookedRoomViewModel
                {
                    RoomId = s.RoomId,
                    RoomName = s.Room.Name ?? "Unknown Room",
                    UserId = s.UserId,
                    UserName = s.User.LastName ?? "Unknown User",
                    StartDate = s.StartDate ?? DateTime.MinValue,
                    EndDate = s.EndDate ?? DateTime.MinValue,
                    NoOfPeople = s.NoOfPeople ?? 0,
                    Status = s.Room.Status ?? "Unknown Status"
                })
                .ToList();

            return data;
        }

        public int ArchiveExpiredBookings()
        {
            int archivedCount = _bookingRepository.ArchiveExpiredBookings();
            _logger.LogInformation("{Count} expired bookings archived.", archivedCount);
            return archivedCount;
        }
        public List<BookingViewModel> GenerateRecurringBookings(BookingViewModel model)
        {
            var bookings = new List<BookingViewModel>();
            var currentDate = model.StartDate;

            while (currentDate <= model.RecurringEndDate)
            {
                var newBooking = new BookingViewModel
                {
                    UserId = model.UserId,
                    RoomId = model.RoomId,
                    StartDate = currentDate,
                    EndDate = currentDate.Add(model.EndDate - model.StartDate),
                    NoOfPeople = model.NoOfPeople,
                    Status = model.Status,
                    IsRecurring = true,
                    Frequency = model.Frequency
                };

                bookings.Add(newBooking);

                // Update the currentDate based on frequency
                switch (model.Frequency.ToLower())
                {
                    case "daily":
                        currentDate = currentDate.AddDays(1);
                        break;
                    case "weekly":
                        currentDate = currentDate.AddDays(7);
                        break;
                    case "monthly":
                        currentDate = currentDate.AddMonths(1);
                        break;
                    default:
                        throw new ArgumentException("Invalid frequency specified");
                }
            }

            return bookings;
        }


    }
}
