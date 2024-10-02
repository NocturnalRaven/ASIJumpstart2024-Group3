using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string UserId { get; set; }
        public string RoomId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string BookedBy { get; set; }
        public DateTime BookedTime{ get; set; }


    }
}
