using EmocineSveikataServer.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmocineSveikataServer.Dto;

namespace EmocineSveikataServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class NotificationController : ControllerBase
	{
		private INotificationService _service;

		public NotificationController(INotificationService service)
		{
			_service = service;
		}

		[HttpGet()]
		public async Task<IActionResult> GetNotifications()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			var notifications = await _service.GetNotificationsAsync(userId);
			return Ok(notifications);
		}

		[HttpPost("mark-read")]
		public async Task<IActionResult> MarkAllAsRead()
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			await _service.MarkAllAsReadAsync(userId);
			return NoContent();
		}

    [HttpPost("send")]
    public async Task<IActionResult> send([FromBody] NotificationDto dto)
    {
			await _service.CreateNotificationAsync(dto.Message, dto.Id);
      return Ok();
    }
  }
}