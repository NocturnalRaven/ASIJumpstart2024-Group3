using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase<RoomController>
    {
        private readonly IRoomService _roomService;

        public RoomController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IRoomService roomService)
            : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
        }
        [HttpGet]
        public IActionResult GetRooms()
        {
            try
            {
                _logger.LogInformation("Fetching all rooms from the service...");
                var rooms = _roomService.GetAllRooms();
                _logger.LogInformation($"Fetched {rooms.Count()} rooms successfully.");
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching rooms: {ex.Message}", ex);
                return StatusCode(500, "Internal server error while fetching rooms.");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            try
            {
                var room = _roomService.GetRoomById(id);
                if (room == null)
                {
                    return NotFound($"Room with ID {id} not found.");
                }

                // Assign static image path based on room style
                room.Image = GetStaticImageForStyle(room.Style);

                return Ok(room);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching room with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error while fetching the room.");
            }
        }

        [HttpPost]
        public IActionResult AddRoom([FromBody] RoomViewModel roomViewModel)
        {
            try
            {
                // Assign static image path based on room style
                roomViewModel.Image = GetStaticImageForStyle(roomViewModel.Style);

                _roomService.AddRoom(roomViewModel);
                return Ok("Room added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding room: {ex.Message}");
                return StatusCode(500, "Internal server error while adding the room.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] RoomViewModel roomViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid room model state");
                return BadRequest(ModelState);
            }

            try
            {
                var existingRoom = _roomService.GetRoomById(id);
                if (existingRoom == null)
                {
                    _logger.LogWarning($"Room with ID {id} not found for update");
                    return NotFound($"Room with ID {id} not found.");
                }

                // Assign static image path based on room style
                roomViewModel.Image = GetStaticImageForStyle(roomViewModel.Style);

                _logger.LogInformation("Updating room with ID {RoomId} with data: {@RoomViewModel}", id, roomViewModel);
                _roomService.UpdateRoom(id, roomViewModel);
                return Ok("Room updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating room with ID {id}: {ex.Message}");
                return StatusCode(500, $"Internal server error while updating room with ID {id}.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            try
            {
                var room = _roomService.GetRoomById(id);
                if (room == null)
                {
                    _logger.LogWarning($"Room with ID {id} not found for deletion");
                    return NotFound($"Room with ID {id} not found.");
                }

                _logger.LogInformation("Deleting room with ID {RoomId}", id);
                _roomService.DeleteRoom(id);
                return Ok("Room deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting room with ID {id}: {ex.Message}");
                return StatusCode(500, $"Internal server error while deleting room with ID {id}.");
            }
        }

        /// <summary>
        /// Returns a static image path based on the room style.
        /// </summary>
        private string GetStaticImageForStyle(string style)
        {
            if (string.IsNullOrEmpty(style))
            {
                return "/img/classroomStyle.jpg"; // Default image
            }

            var normalizedStyle = style.ToLower();
            var imagePath = normalizedStyle switch
            {
                "classroom" => "/img/classroomStyle.jpg",
                "banquet" => "/img/banquet.jpg",
                "u-shape" => "/img/ushape.jpeg",
                "conference" => "/img/conferenceStyle.png",
                "auditorium" => "/img/auditorium.jpg",
                "boardroom" => "/img/boardroomStyle.jpeg",
                _ => null
            };

            if (imagePath == null)
            {
                _logger.LogWarning($"Unrecognized room style: {style}. Using default image.");
                return "/img/classroomStyle.jpg"; // Default image for unknown styles
            }

            return imagePath;
        }

    }
}
