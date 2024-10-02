using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IBookingRepository
    {
        IQueryable<MBooking> GetBooking();
        bool BookingExists(int bookingId);
        void AddBooking(MBooking booking);
        void UpdateBooking(MBooking booking);
        void DeleteBooking(int bookingId);
    }
}
