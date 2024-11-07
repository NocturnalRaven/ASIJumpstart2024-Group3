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
            return this.GetDbSet<Booking>();
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

    }
}
