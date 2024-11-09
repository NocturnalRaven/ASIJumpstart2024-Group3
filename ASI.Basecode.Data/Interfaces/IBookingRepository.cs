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
        IQueryable<Booking> GetBooking();
        bool BookingExists(int bookingId);
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int bookingId);
        int ArchiveExpiredBookings();
    }
}
