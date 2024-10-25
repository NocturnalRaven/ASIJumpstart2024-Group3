using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomReposity;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomReposity = roomRepository;
        }
    }
}
