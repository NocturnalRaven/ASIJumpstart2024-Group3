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

        public IQueryable<MBooking> GetBooking()
        {
            return this.GetDbSet<MBooking>();
        }

        public bool BookingExists(int bookingId)
        {
            return this.GetDbSet<MBooking>().Any(x => x.BookingId == bookingId);
        }

        public void AddUser(MBooking booking)
        {
            var maxId = this.GetDbSet<MBooking>().Max(x => x.BookingId) + 1;
            booking.BookingId = maxId;
            booking.UpdDt = DateTime.Now;
            this.GetDbSet<MBooking>().Add(booking);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(MBooking booking)
        {
            this.GetDbSet<MBooking>().Update(booking);
            booking.UpdDt = DateTime.Now;
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(int bookingId)
        {
            var userToDelete = this.GetDbSet<MBooking>().FirstOrDefault(x => x.Deleted != true && x.BookingId == bookingId);
            if (userToDelete != null)
            {
                userToDelete.Deleted = true;
                userToDelete.UpdDt = DateTime.Now;
            }
            UnitOfWork.SaveChanges();
        }

    }
}
