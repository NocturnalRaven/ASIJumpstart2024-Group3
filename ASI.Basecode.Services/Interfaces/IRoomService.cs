using System;
using System.Collections.Generic;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<RoomViewModel> RetrieveAll(int? id = null, string name = null);

        RoomViewModel RetrieveRoom(int id);

        void Add(RoomViewModel model);

        void Update(RoomViewModel model);

        void Delete(int id);    
    }
}
