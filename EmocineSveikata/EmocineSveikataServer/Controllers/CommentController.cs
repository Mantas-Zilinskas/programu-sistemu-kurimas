using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Services.CommentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmocineSveikataServer.Controllers
{
	[Route("api/discussions")]
	[ApiController]
	[Authorize]
	public class CommentController : ControllerBase
	{
		private readonly ICommentService _service;
		private readonly ILogger<CommentController> _logger;

		public CommentController(ICommentService service, ILogger<CommentController> logger)
		{
			_service = service;
			_logger = logger;
		}

		[HttpPost("{discussionId}/comments/{commentId}/like")]
		public async Task<IActionResult> LikeCommentAsync(int commentId)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			return Ok(await _service.ChangeLikeStatusCommentAsync(commentId, userId));
		}

		[HttpPost("{discussionId}/comments/{commentId}/reply")]
		public async Task<IActionResult> ReplyToCommentAsync(int discussionId, int commentId, [FromBody] CommentCreateDto reply)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			return Ok(await _service.ReplyToCommentAsync(discussionId, commentId, reply, userId));
		}

		[HttpPut("{discussionId}/comments/{commentId}")]
		public async Task<IActionResult> EditCommentAsync(int commentId, [FromBody] CommentUpdateDto comment)
		{
			return Ok(await _service.UpdateCommentAsync(commentId, comment));
		}

		[HttpDelete("{discussionId}/comments/{commentId}")]
		public async Task<IActionResult> DeleteCommentAsync(int commentId)
		{
			await _service.DeleteCommentAsync(commentId);
			return NoContent();
		}

	}
}
