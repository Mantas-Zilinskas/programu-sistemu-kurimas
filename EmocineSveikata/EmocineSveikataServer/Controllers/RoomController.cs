using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Dto.RoomDtos;
using EmocineSveikataServer.Services.RoomService;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("book/{roomId}")]
        [Authorize]
        public async Task<IActionResult> BookRoom(int roomId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

				var meetLink = await _roomService.BookRoomAsync(roomId, userId);

				return Ok(new
				{
					message = "Kambarys rezervuotas!",
					meetLink
				});
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Rezervacija nepavyko: ", error = ex.Message });
			}
		}
	}
}
