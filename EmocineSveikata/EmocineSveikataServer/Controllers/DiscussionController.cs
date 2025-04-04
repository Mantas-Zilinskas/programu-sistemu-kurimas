using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Services.DiscussionService;

namespace EmocineSveikataServer.Controllers
{
	[Route("api/discussions")]
	[ApiController]
	public class DiscussionsController : ControllerBase
	{
		private readonly IDiscussionService _service;
		private readonly ILogger<DiscussionsController> _logger;

		public DiscussionsController(ILogger<DiscussionsController> logger, IDiscussionService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetDiscussions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			return Ok(await _service.GetPagedDiscussionsAsync(page, pageSize));
		}

		[HttpGet("{discussionId}")]
		public async Task<IActionResult> GetDiscussion(int discussionId)
		{
			var discussion = await _service.GetDiscussionAsync(discussionId);
			if (discussion == null) return NotFound();
			return Ok(discussion);
		}

		[HttpPost]
		public async Task<IActionResult> CreateDiscussion([FromBody] Discussion discussion)
		{
			await _service.CreateDiscussionAsync(discussion);
			return CreatedAtAction(nameof(GetDiscussion), new { discussionId = discussion.Id }, discussion);
		}

		[HttpPost("{discussionId}/like")]
		public async Task<IActionResult> LikeDiscussion(int discussionId)
		{
			return Ok(await _service.AddLikeAsync(discussionId));
		}

		[HttpDelete("{discussionId}")]
		public async Task<IActionResult> DeleteDiscussionAsync(int discussionId)
		{
			await _service.DeleteDiscussionAsync(discussionId);
			return NoContent();
		}
	}
}
