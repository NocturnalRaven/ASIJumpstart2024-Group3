using System;
using System.Collections.Generic;

namespace ASI.Basecode.WebApp.Models
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int Floor { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
