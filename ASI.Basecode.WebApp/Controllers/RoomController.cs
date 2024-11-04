﻿using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : ControllerBase<RoomController>
    {
        private readonly IRoomService _roomService;

        public RoomController(
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IMapper mapper,
            IRoomService roomService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _roomService = roomService;
        }

        [HttpGet("GetRooms")]
        public IActionResult GetRooms()
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms);
        }
    }
}
