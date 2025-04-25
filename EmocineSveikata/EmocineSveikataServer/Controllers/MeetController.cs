using EmocineSveikataServer.Services.Meets;
using Microsoft.AspNetCore.Mvc;

namespace EmocineSveikataServer.Controllers
{
    [ApiController]
	[Route("api/meet")]
	public class MeetController : ControllerBase
	{
		private readonly GoogleMeetService _meetService;

		public MeetController(GoogleMeetService googleMeetService)
		{
			_meetService = googleMeetService;
		}

		[HttpGet("create-meet")]
		public async Task<IActionResult> CreateLink()
		{
			var link = await _meetService.CreateMeetAsync();
			return Ok(new { link });
		}
	}

}
