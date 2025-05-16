using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Services.DiscussionService;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Controllers
{
	[Route("api/discussions")]
	[ApiController]
	[Authorize]
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
		[AllowAnonymous]
		public async Task<IActionResult> GetDiscussions([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] DiscussionTagEnum? tag = null, [FromQuery] bool isPopular = false)
		{
			return Ok(await _service.GetPagedDiscussionsAsync(page, pageSize, tag, isPopular));
		}

		[HttpGet("tags")]
		[AllowAnonymous]
		public IActionResult GetTags()
		{
			var tagStrings = _service.GetAllTags();

			return Ok(tagStrings);
		}

		[HttpGet("{discussionId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetDiscussion(int discussionId)
		{
			int? userId = null;

			if (User.Identity?.IsAuthenticated == true)
			{
				var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim != null)
					userId = int.Parse(userIdClaim.Value);
			}

			var discussion = await _service.GetDiscussionAsync(discussionId, userId);
			if (discussion == null) return NotFound();
			return Ok(discussion);
		}

		[HttpPost]
		public async Task<IActionResult> CreateDiscussion([FromBody] DiscussionCreateDto discussionDto)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			discussionDto.CreatorUserId = userId;
			var newDto = await _service.CreateDiscussionAsync(discussionDto);
			return CreatedAtAction(nameof(GetDiscussion), new { discussionId = newDto.Id }, newDto);
		}

		[HttpPut("{discussionId}/force")]
		public async Task<IActionResult> ForceEditDiscussion(int discussionId, [FromBody] DiscussionUpdateDto discussionDto)
		{
			var updatedDto = await _service.ForceUpdateDiscussionAsync(discussionId, discussionDto);
			if (updatedDto == null) return NotFound();
			return Ok(updatedDto);
		}

		[HttpPut("{discussionId}")]
		public async Task<IActionResult> EditDiscussion(int discussionId, [FromBody] DiscussionUpdateDto discussionDto)
		{
			DiscussionDto updatedDto;
			try
			{
				updatedDto = await _service.UpdateDiscussionAsync(discussionId, discussionDto);
			}
			catch (DbUpdateConcurrencyException)
			{
				return Conflict(new { message = "The item you attempted to edit was already modified." });
			}
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
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			comment.CreatorUserId = userId;
			var discussionDto = await _service.AddCommentToDiscussionAsync(discussionId, comment);
			if (discussionDto == null) return NotFound();
			return Ok(discussionDto);
		}

		[HttpPost("{discussionId}/like")]
		public async Task<IActionResult> LikeDiscussion(int discussionId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			var discussionDto = await _service.ChangeLikeStatusAsync(discussionId, userId);
			if (discussionDto == null) return NotFound();
			return Ok(discussionDto);
		}
	}
}
