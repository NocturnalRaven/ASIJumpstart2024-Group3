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
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository repository, IMapper mapper, ILogger<BookingService> logger)
        {
            _mapper = mapper;
            _bookingRepository = repository;
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
        public IEnumerable<BookingViewModel> GenerateRecurringBookings(BookingViewModel model)
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

    }
}
