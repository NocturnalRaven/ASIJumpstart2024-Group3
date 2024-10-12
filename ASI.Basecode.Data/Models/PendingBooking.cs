using System;
using System.Collections.Generic;

namespace ASI.Basecode.WebApp.Models
{
    public partial class PendingBooking
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
