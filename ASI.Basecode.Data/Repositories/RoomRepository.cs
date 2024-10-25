using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IQueryable<Room> GetRooms()
        {
            return this.GetDbSet<Room>();
        }

        public bool RoomExists(int roomId)
        {
            return this.GetDbSet<Room>().Any(x => x.Id == roomId);
        }

        public void AddRoom(Room room)
        {
            var maxId = this.GetDbSet<Room>().Select(x => (int?)x.Id).ToList().DefaultIfEmpty(0).Max() + 1;
            room.Id = maxId ?? 0;
            room.CreatedAt = DateTime.Now;
            room.UpdatedAt = DateTime.Now;
            room.Deleted = false;
            this.GetDbSet<Room>().Add(room);
            UnitOfWork.SaveChanges();
        }

        public void UpdateRoom(Room room)
        {
            this.GetDbSet<Room>().Update(room);
            room.UpdatedAt = DateTime.Now;
            UnitOfWork.SaveChanges();
        }

        public void DeleteRoom(int roomId)
        {
            var roomToDelete = this.GetDbSet<Room>().FirstOrDefault(x => x.Deleted != true && x.Id == roomId);
            if (roomToDelete != null)
            {
                roomToDelete.Deleted = true;
                roomToDelete.DeletedAt = DateTime.Now;
            }
            UnitOfWork.SaveChanges();
        }
    }
}
