using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Services.DiscussionService;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Enums;

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
		public async Task<IActionResult> GetDiscussions([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] DiscussionTagEnum? tag = null)
		{
			return Ok(await _service.GetPagedDiscussionsAsync(page, pageSize, tag));
		}

		[HttpGet("tags")]
		public IActionResult GetTags()
		{
			var tagStrings = _service.GetAllTags();

			return Ok(tagStrings);
		}

		[HttpGet("{discussionId}")]
		public async Task<IActionResult> GetDiscussion(int discussionId)
		{
			var discussion = await _service.GetDiscussionAsync(discussionId);
			if (discussion == null) return NotFound();
			return Ok(discussion);
		}

		[HttpPost]
		public async Task<IActionResult> CreateDiscussion([FromBody] DiscussionCreateDto discussionDto)
		{
			var newDto = await _service.CreateDiscussionAsync(discussionDto);
			return CreatedAtAction(nameof(GetDiscussion), new { discussionId = newDto.Id }, newDto);
		}

		[HttpPut("{discussionId}")]
		public async Task<IActionResult> EditDiscussion(int discussionId, [FromBody] DiscussionUpdateDto discussionDto)
		{
			var updatedDto = await _service.UpdateDiscussionAsync(discussionId, discussionDto);
			if (updatedDto == null) return NotFound();
			return Ok(updatedDto);
		}

		[HttpDelete("{discussionId}")]
		public async Task<IActionResult> DeleteDiscussionAsync(int discussionId)
		{
			await _service.DeleteDiscussionAsync(discussionId);
			return NoContent();
		}

		[HttpPost("{discussionId}/comments")]
		public async Task<IActionResult> AddCommentAsync(int discussionId, [FromBody] CommentCreateDto comment)
		{
			var discussionDto = await _service.AddCommentToDiscussionAsync(discussionId, comment);
			if (discussionDto == null) return NotFound();
			return Ok(discussionDto);
		}

		[HttpPost("{discussionId}/like")]
		public async Task<IActionResult> LikeDiscussion(int discussionId)
		{
			var discussionDto = await _service.AddLikeAsync(discussionId);
			if (discussionDto == null) return NotFound();
			return Ok(discussionDto);
		}

	}
}
