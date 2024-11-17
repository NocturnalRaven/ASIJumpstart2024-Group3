using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }

        public IQueryable<Booking> GetBooking()
        {
            var bookings = this.GetDbSet<Booking>();
            Console.WriteLine($"Total Bookings Retrieved: {bookings.Count()}"); // Log the count for debugging
            return bookings;
        }


        public bool BookingExists(int bookingId)
        {
            return this.GetDbSet<Booking>().Any(x => x.Id == bookingId);
        }

        public void AddBooking(Booking booking)
        {
            booking.CreatedAt = DateTime.Now;
            this.GetDbSet<Booking>().Add(booking);
            UnitOfWork.SaveChanges(); // The database will handle ID assignment
        }


        public void UpdateBooking(Booking booking)
        {
            this.GetDbSet<Booking>().Update(booking);
            booking.UpdatedAt = DateTime.Now;
            UnitOfWork.SaveChanges();
        }

        public void DeleteBooking(int bookingId)
        {
            var bookingToDelete = this.GetDbSet<Booking>().FirstOrDefault(x => x.Deleted != true && x.Id == bookingId);
            if (bookingToDelete != null)
            {
                bookingToDelete.Deleted = true;
                bookingToDelete.UpdatedAt = DateTime.Now;
            }
            UnitOfWork.SaveChanges();
        }
        /// <summary>
        /// Archives expired bookings by setting their status to "Archived".
        /// </summary>
        /// <returns>The count of bookings archived.</returns>
        public int ArchiveExpiredBookings()
        {
            var expiredBookings = this.GetDbSet<Booking>()
                .Where(b => b.EndDate < DateTime.Now && b.Status != "Archived" && !b.Deleted);

            int archivedCount = 0;
            foreach (var booking in expiredBookings)
            {
                booking.Status = "Archived";
                booking.UpdatedAt = DateTime.Now;
                archivedCount++;
            }

            UnitOfWork.SaveChanges(); // Save changes after updating statuses
            Console.WriteLine($"{archivedCount} expired bookings have been archived."); // Log the count for confirmation

            return archivedCount;
        }
    }
}
