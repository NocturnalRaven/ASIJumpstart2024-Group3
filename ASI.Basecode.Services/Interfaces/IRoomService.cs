using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<RoomViewModel> GetAllRooms();
    }
}
