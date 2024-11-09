using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<RoomViewModel> GetAllRooms();
        RoomViewModel GetRoomById(int id);
        void AddRoom(RoomViewModel roomViewModel);
        void UpdateRoom(int id, RoomViewModel roomViewModel);
        void DeleteRoom(int id);
    }
}
