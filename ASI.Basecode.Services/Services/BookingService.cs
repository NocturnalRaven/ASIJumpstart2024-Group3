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

        public BookingService(IBookingRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _bookingRepository = repository;
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

        /// <summary>
        /// Retrieves all booked rooms with additional details.
        /// </summary>
        public IEnumerable<BookedRoomViewModel> RetrieveBookedRooms()
        {
            var data = _bookingRepository.GetBooking()
                .Where(x => !x.Deleted && x.Room.Status == "Booked") // Check Room status instead of Booking status
                .Include(b => b.Room)    // Load Room details
                .Include(b => b.User)    // Load User details
                .Select(s => new BookedRoomViewModel
                {
                    RoomId = s.RoomId,
                    RoomName = s.Room.Name ?? "Unknown Room",
                    UserId = s.UserId,
                    UserName = s.User.LastName ?? "Unknown User",
                    StartDate = s.StartDate ?? DateTime.MinValue,
                    EndDate = s.EndDate ?? DateTime.MinValue,
                    NoOfPeople = s.NoOfPeople ?? 0,
                    Status = s.Room.Status ?? "Unknown Status" // Display Room's Status
                })
                .ToList(); // Materialize the results to ensure query execution

            return data;
        }


    }
}
