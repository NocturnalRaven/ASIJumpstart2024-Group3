using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

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
                var rooms = _roomService.GetAllRooms();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching rooms: {ex.Message}");
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
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid room model state");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Adding room with data: {@RoomViewModel}", roomViewModel);
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
    }
}
