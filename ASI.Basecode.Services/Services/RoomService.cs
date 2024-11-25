using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public IEnumerable<RoomViewModel> GetAllRooms()
        {
            var rooms = _roomRepository.GetRooms()
                .Where(r => !r.Deleted)
                .AsEnumerable() // Switch to in-memory evaluation for projection
                .Select(room => new RoomViewModel
                {
                    RoomId = room.Id,
                    Name = room.Name,
                    Style = room.Style,
                    Capacity = room.Capacity,
                    Status = room.Status,
                    Floor = room.Floor,
                    CreatedAt = room.CreatedAt,
                    UpdatedAt = room.UpdatedAt,
                    Deleted = room.Deleted,
                    DeletedAt = room.DeletedAt,
                    Image = GetImagePath(room.Style) // Call the static method here
                });

            return rooms;
        }


        public RoomViewModel GetRoomById(int id)
        {
            var room = _roomRepository.GetRooms()
                .FirstOrDefault(r => r.Id == id && !r.Deleted);

            if (room == null) return null;

            return new RoomViewModel
            {
                RoomId = room.Id,
                Name = room.Name,
                Style = room.Style,
                Capacity = room.Capacity,
                Status = room.Status,
                Floor = room.Floor,
                CreatedAt = room.CreatedAt,
                UpdatedAt = room.UpdatedAt,
                Deleted = room.Deleted,
                DeletedAt = room.DeletedAt,
                Image = GetImagePath(room.Style) // Call the static method here
            };
        }


        public void AddRoom(RoomViewModel roomViewModel)
        {
            var room = new Room
            {
                Name = roomViewModel.Name,
                Style = roomViewModel.Style,
                Capacity = roomViewModel.Capacity,
                Status = roomViewModel.Status,
                Floor = roomViewModel.Floor,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deleted = false
            };

            _roomRepository.AddRoom(room);
        }

        public void UpdateRoom(int id, RoomViewModel roomViewModel)
        {
            var room = _roomRepository.GetRooms().FirstOrDefault(r => r.Id == id && !r.Deleted);
            if (room != null)
            {
                room.Name = roomViewModel.Name;
                room.Style = roomViewModel.Style;
                room.Capacity = roomViewModel.Capacity;
                room.Status = roomViewModel.Status;
                room.Floor = roomViewModel.Floor;
                room.UpdatedAt = DateTime.Now;

                _roomRepository.UpdateRoom(room);
            }
            else
            {
                throw new InvalidOperationException("Room not found.");
            }
        }

        public void DeleteRoom(int id)
        {
            var room = _roomRepository.GetRooms().FirstOrDefault(r => r.Id == id && !r.Deleted);
            if (room != null)
            {
                room.Deleted = true;
                room.DeletedAt = DateTime.Now;
                _roomRepository.UpdateRoom(room);
            }
            else
            {
                throw new InvalidOperationException("Room not found.");
            }
        }

        /// <summary>
        /// Returns the image path based on the room style.
        /// </summary>
        /// <param name="style">Room style</param>
        /// <returns>Image path</returns>
        private static string GetImagePath(string style)
        {
            if (string.IsNullOrEmpty(style))
            {
                return "/img/classroomStyle.jpg"; // Default image
            }

            return style.ToLower() switch
            {
                "classroom" => "/img/classroomStyle.jpg",
                "banquet" => "/img/banquet.jpg",
                "u-shape" => "/img/ushape.jpeg",
                "conference" => "/img/conferenceStyle.png",
                "auditorium" => "/img/auditorium.jpg",
                "boardroom" => "/img/boardroomStyle.jpeg",
                _ => "/img/classroomStyle.jpg" // Fallback to default image
            };
        }
    }
}
