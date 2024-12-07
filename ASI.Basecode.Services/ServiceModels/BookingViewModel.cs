using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Room ID is required")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        [CompareDates("StartDate", ErrorMessage = "End date must be after the start date")]
        public DateTime EndDate { get; set; }

        public int NoOfPeople { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; }

        public bool IsRecurring { get; set; }

        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters")]
        [RequiredIf("IsRecurring", true, ErrorMessage = "Frequency is required for recurring bookings")]
        public string Frequency { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        [RequiredIf("IsRecurring", true, ErrorMessage = "Recurring end date is required for recurring bookings")]
        [CompareDates("StartDate", ErrorMessage = "Recurring end date must be after the start date")]
        public DateTime? RecurringEndDate { get; set; }
    }
}
