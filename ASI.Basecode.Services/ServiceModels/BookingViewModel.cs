using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        [Required(ErrorMessage = "This is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "This is required")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "This is required")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfPeople { get; set; }
        public string Status { get; set; }
        public bool IsRecurring { get; set; }
        public string Frequency { get; set; }
    }
}
