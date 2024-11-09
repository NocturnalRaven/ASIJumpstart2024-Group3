using System;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookedRoomViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfPeople { get; set; }
        public string Status { get; set; }
    }
}
