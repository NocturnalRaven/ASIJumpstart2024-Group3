using System;
using System.Collections.Generic;

namespace ASI.Basecode.WebApp.Models
{
    public partial class Booking
    {
        public Booking()
        {
            PendingBookings = new HashSet<PendingBooking>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfPeople { get; set; }
        public string Status { get; set; }
        public bool IsRecurring { get; set; }
        public string Frequency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Deleted{ get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PendingBooking> PendingBookings { get; set; }
    }
}
