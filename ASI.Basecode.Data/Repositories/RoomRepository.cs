using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        // Retrieves all rooms, including both active and deleted rooms if needed
        public IQueryable<Room> GetRooms()
        {
            return this.GetDbSet<Room>();
        }

        // Checks if a room with the specified ID exists
        public bool RoomExists(int roomId)
        {
            return this.GetDbSet<Room>().Any(x => x.Id == roomId);
        }

        // Adds a new room to the database
        public void AddRoom(Room room)
        {
            // Do not set room.Id manually; let the database handle the auto-increment
            room.CreatedAt = DateTime.Now;
            room.UpdatedAt = DateTime.Now;
            this.GetDbSet<Room>().Add(room);
            UnitOfWork.SaveChanges();
        }

        // Updates the information of an existing room
        public void UpdateRoom(Room room)
        {
            room.UpdatedAt = DateTime.Now;
            this.GetDbSet<Room>().Update(room);
            UnitOfWork.SaveChanges();
        }

        // Soft-deletes a room by marking it as deleted and setting the DeletedAt timestamp
        public void DeleteRoom(int roomId)
        {
            var roomToDelete = this.GetDbSet<Room>().FirstOrDefault(x => !x.Deleted && x.Id == roomId);
            if (roomToDelete != null)
            {
                roomToDelete.Deleted = true;
                roomToDelete.DeletedAt = DateTime.Now;
                UnitOfWork.SaveChanges();
            }
        }
    }
}
