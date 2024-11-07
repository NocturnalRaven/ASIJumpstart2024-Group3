using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

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
                .Select(s => new BookingViewModel
                {
                    BookingId = s.Id,
                    UserId = s.UserId,
                    RoomId = s.RoomId,
                    StartDate = s.StartDate ?? DateTime.MinValue,
                    EndDate = s.EndDate ?? DateTime.MinValue,
                    NoOfPeople = s.NoOfPeople ?? 0,
                    Status = s.Status,
                    IsRecurring = s.IsRecurring ?? false,
                    Frequency = s.Frequency
                });

            return data;
        }

        public BookingViewModel RetrieveBooking(int id)
        {
            var data = _bookingRepository.GetBooking().FirstOrDefault(x => !x.Deleted && x.Id == id);
            if (data == null) return null;

            return new BookingViewModel
            {
                BookingId = data.Id,
                UserId = data.UserId,
                RoomId = data.RoomId,
                StartDate = data.StartDate ?? DateTime.MinValue,
                EndDate = data.EndDate ?? DateTime.MinValue,
                NoOfPeople = data.NoOfPeople ?? 0,
                Status = data.Status,
                IsRecurring = data.IsRecurring ?? false,
                Frequency = data.Frequency
            };
        }

        public void Add(BookingViewModel model)
        {
            var newModel = new Booking
            {
                UserId = model.UserId,
                RoomId = model.RoomId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                NoOfPeople = model.NoOfPeople,
                Status = model.Status,
                IsRecurring = model.IsRecurring,
                Frequency = model.Frequency,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _bookingRepository.AddBooking(newModel);
        }

        public void Update(BookingViewModel model)
        {
            var existingData = _bookingRepository.GetBooking()
                .FirstOrDefault(s => !s.Deleted && s.Id == model.BookingId);

            if (existingData != null)
            {
                existingData.UserId = model.UserId;
                existingData.RoomId = model.RoomId;
                existingData.StartDate = model.StartDate;
                existingData.EndDate = model.EndDate;
                existingData.NoOfPeople = model.NoOfPeople;
                existingData.Status = model.Status;
                existingData.IsRecurring = model.IsRecurring;
                existingData.Frequency = model.Frequency;

                _bookingRepository.UpdateBooking(existingData);
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
                .Where(x => !x.Deleted && x.Status == "Booked") // Only retrieve rooms marked as "Booked"
                .Select(s => new BookedRoomViewModel
                {
                    RoomId = s.RoomId,
                    RoomName = s.Room.Name, // Assuming Room navigation property is available
                    UserId = s.UserId,
                    UserName = s.User.LastName, // Assuming User navigation property is available
                    StartDate = s.StartDate ?? DateTime.MinValue,
                    EndDate = s.EndDate ?? DateTime.MinValue,
                    NoOfPeople = s.NoOfPeople ?? 0,
                    Status = s.Status
                });

            return data;
        }
    }
}
