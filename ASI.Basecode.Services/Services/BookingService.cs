using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

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
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    NoOfPeople = s.NoOfPeople,
                    Status = s.Status,
                    IsRecurring = s.IsRecurring,
                    Frequency = s.Frequency
                });

            return data;
        }


        public BookingViewModel RetrieveBooking(int id)
        {
            var data = _bookingRepository.GetBooking().FirstOrDefault(x => x.Deleted != true && x.UserId == id);
            var model = new BookingViewModel
            {
                BookingId = data.Id,
                UserId = data.UserId,
                RoomId = data.RoomId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                NoOfPeople = data.NoOfPeople,
                Status = data.Status,
                IsRecurring = data.IsRecurring,
                Frequency = data.Frequency
            };
            return model;
        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Add(BookingViewModel model)
        {
            var newModel = new Booking();
            newModel.Id = model.BookingId;
            newModel.UserId = model.UserId;
            newModel.RoomId = model.RoomId;
            newModel.StartDate = model.StartDate;
            newModel.EndDate = model.EndDate;
            newModel.NoOfPeople = model.NoOfPeople;
            newModel.Status = model.Status;
            newModel.IsRecurring = model.IsRecurring;
            newModel.Frequency = model.Frequency;
            _bookingRepository.AddBooking(newModel);
        }

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Update(BookingViewModel model)
        {
            var existingData = _bookingRepository.GetBooking().Where(s => s.Deleted != true && s.UserId == model.BookingId).FirstOrDefault();
            existingData.Id = model.BookingId;
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

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            _bookingRepository.DeleteBooking(id);
        }
    }
}
