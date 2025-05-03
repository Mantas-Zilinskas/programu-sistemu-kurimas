using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Dto.RoomDtos;
using EmocineSveikataServer.Services.RoomService;

namespace EmocineSveikataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<RoomDto>>> GetAllAvailableRooms()
        {
            var rooms = await _roomService.GetAllCurrentRooms();

            return Ok(rooms);
        }
    }
}
