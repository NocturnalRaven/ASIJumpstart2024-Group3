using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface  IBookingService
    {
        IEnumerable<BookingViewModel> RetrieveAll(int? bookingId = null, int? userId = null, DateTime? startDate = null, string status = null);
        BookingViewModel RetrieveBooking(int id);
        void Add(BookingViewModel model);
        void Update(BookingViewModel model);
        void Delete(int id);

    }
}
