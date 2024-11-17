using System;

public class RoomViewModel
{
    public int RoomId { get; set; }
    public string Name { get; set; }
    public string Style { get; set; }
    public int? Capacity { get; set; }
    public string Status { get; set; }
    public int? Floor { get; set; }
    public string Image { get; set; } // Now writable
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
