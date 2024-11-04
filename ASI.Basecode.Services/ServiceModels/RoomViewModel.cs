using System;

namespace ASI.Basecode.Services.ServiceModels
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }        // Maps to Room.Id
        public string Name { get; set; }
        public string Style { get; set; }
        public int? Capacity { get; set; }
        public string Status { get; set; }
        public int? Floor { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
