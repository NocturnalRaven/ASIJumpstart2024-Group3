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
        public string Status { get; set; }

        public string Frequency { get; set; }
    }

    public class BookingListViewModel
    {
        [DisplayName("ID")]
        public string IdFilter { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resources.Views.Screen))]
        public string FirstNameFilter { get; set; }

        public IEnumerable<UserViewModel> dataList { get; set; }
    }
}
