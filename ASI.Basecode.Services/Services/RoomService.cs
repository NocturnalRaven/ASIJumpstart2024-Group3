using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomReposity;

        private readonly IMapper _mapper;
        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _mapper = mapper;
            _roomReposity = roomRepository;
        }

        public IEnumerable<RoomViewModel> RetrieveAll(int? id = null, string name = null)
        {
            var data = _roomReposity.GetRooms()
                .Where(x => x.Deleted != true
                        && (!id.HasValue || x.Id == id)
                        && (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .Select(s => new RoomViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Style = s.Style,
                    Capacity = s.Capacity,
                    Status = s.Status,
                    Floor = s.Floor,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    Deleted = s.Deleted,
                    DeletedAt = s.DeletedAt
                });
            return data;
        }

        public RoomViewModel RetrieveRoom(int id)
        {
            var data = _roomReposity.GetRooms().FirstOrDefault(x => x.Deleted != true && x.Id == id);
            var model = new RoomViewModel
            {
                Id = data.Id,
                Name = data.Name,
                Style = data.Style,
                Capacity = data.Capacity,
                Status = data.Status,
                Floor = data.Floor
            };
            return model;
        }

        public void Add(RoomViewModel model)
        {
            var data = new Room();
            data.Name = model.Name;
            data.Style = model.Style;
            data.Capacity = model.Capacity;
            // data.Status = model.Status;
            data.Floor = model.Floor;
            data.Details = model.Details;
            // data.Image = model.Image;

            _roomReposity.AddRoom(data);
        }

        public void Update(RoomViewModel model)
        {
            var data = _roomReposity.GetRooms().FirstOrDefault(x => x.Deleted != true && x.Id == model.Id);
            if (data != null)
            {
                data.Name = model.Name;
                data.Style = model.Style;
                data.Capacity = model.Capacity;
                data.Status = model.Status;
                data.Floor = model.Floor;
                // data.Image = model.Image;

                _roomReposity.UpdateRoom(data);
            }
        }

        public void Delete(int id)
        {
            _roomReposity.DeleteRoom(id);
        }
    }
}
