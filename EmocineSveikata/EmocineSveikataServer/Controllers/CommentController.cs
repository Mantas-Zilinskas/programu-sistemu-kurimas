using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Services.CommentService;
using Microsoft.AspNetCore.Mvc;

namespace EmocineSveikataServer.Controllers
{
	[Route("api/discussions")]
	[ApiController]
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
			return Ok(await _service.LikeCommentAsync(commentId));
		}

		[HttpPost("{discussionId}/comments/{commentId}/reply")]
		public async Task<IActionResult> ReplyToCommentAsync(int discussionId, int commentId, [FromBody] CommentCreateDto reply)
		{
			return Ok(await _service.ReplyToCommentAsync(discussionId, commentId, reply));
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
