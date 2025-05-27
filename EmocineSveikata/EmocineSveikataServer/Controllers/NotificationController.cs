using EmocineSveikataServer.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null) return Unauthorized();
			var userId = int.Parse(userIdClaim.Value);
			var notifications = await _service.GetNotificationsAsync(userId);
			return Ok(notifications);
		}

		[HttpPost("mark-read")]
		public async Task<IActionResult> MarkAllAsRead()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null) return Unauthorized();
			var userId = int.Parse(userIdClaim.Value);
			await _service.MarkAllAsReadAsync(userId);
			return NoContent();
		}

	}
}