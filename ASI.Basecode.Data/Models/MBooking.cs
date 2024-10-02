using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class MBooking
    {
        public int BookingId { get; set; }
        public string InsBy { get; set; }
        public DateTime InsDt { get; set; }
        public string UpdBy { get; set; }
        public DateTime UpdDt { get; set; }
        public bool Deleted { get; set; }
        public DateTime Duration { get; set; }

        public Boolean isReccuring { get; set; }
    }
}
