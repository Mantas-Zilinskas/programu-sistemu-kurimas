using Microsoft.AspNetCore.Mvc;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Data;

namespace EmocineSveikataServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DiscussionsController : ControllerBase
	{
		private static readonly List<DiscussionModel> _discussions = new();
		private readonly ILogger<DiscussionsController> _logger;
		private readonly DataContext _context;

		public DiscussionsController(ILogger<DiscussionsController> logger, DataContext context)
		{
			_logger = logger;
			_context = context;
		}
		[HttpGet]
		public IActionResult GetDiscussions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var paginatedDiscussions = _discussions.Where(d => !d.IsDeleted)
													.Skip((page - 1) * pageSize)
													.Take(pageSize)
													.ToList();
			return Ok(paginatedDiscussions);
		}

		[HttpGet("{id}")]
		public IActionResult GetDiscussion(int id)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == id && !d.IsDeleted);
			if (discussion == null) return NotFound();
			return Ok(discussion);
		}

		[HttpPost]
		public IActionResult CreateDiscussion([FromBody] DiscussionModel discussion)
		{
			discussion.Id = _discussions.Count + 1;
			_discussions.Add(discussion);
			return CreatedAtAction(nameof(GetDiscussion), new { id = discussion.Id }, discussion);
		}

		[HttpPost("{id}/like")]
		public IActionResult LikeDiscussion(int id)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == id && !d.IsDeleted);
			if (discussion == null) return NotFound();
			discussion.Likes++;
			return Ok(discussion);
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteDiscussion(int id)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == id);
			if (discussion == null) return NotFound();

			discussion.IsDeleted = true;
			return NoContent();
		}

		[HttpPost("{discussionId}/comments")]
		public IActionResult AddComment(int discussionId, [FromBody] CommentModel comment)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId && !d.IsDeleted);
			if (discussion == null) return NotFound();

			comment.Id = discussion.Comments.Count + 1;
			discussion.Comments.Add(comment);
			return Ok(discussion);
		}

		[HttpPost("{discussionId}/comments/{commentId}/like")]
		public IActionResult LikeComment(int discussionId, int commentId)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId && !d.IsDeleted);
			if (discussion == null) return NotFound();

			var comment = discussion.Comments.FirstOrDefault(c => c.Id == commentId && !c.IsDeleted);
			if (comment == null) return NotFound();

			comment.Likes++;
			return Ok(comment);
		}

		[HttpPost("{discussionId}/comments/{commentId}/reply")]
		public IActionResult ReplyToComment(int discussionId, int commentId, [FromBody] CommentModel reply)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId && !d.IsDeleted);
			if (discussion == null) return NotFound();

			var comment = discussion.Comments.FirstOrDefault(c => c.Id == commentId && !c.IsDeleted);
			if (comment == null) return NotFound();

			reply.Id = comment.Replies.Count + 1;
			comment.Replies.Add(reply);
			return Ok(comment);
		}

		[HttpPut("{discussionId}/comments/{commentId}")]
		public IActionResult EditComment(int discussionId, int commentId, [FromBody] string newContent)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId && !d.IsDeleted);
			if (discussion == null) return NotFound();

			var comment = discussion.Comments.FirstOrDefault(c => c.Id == commentId && !c.IsDeleted);
			if (comment == null) return NotFound();

			comment.Content = newContent;
			return Ok(comment);
		}

		[HttpDelete("{discussionId}/comments/{commentId}")]
		public IActionResult DeleteComment(int discussionId, int commentId)
		{
			var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
			if (discussion == null) return NotFound();

			var comment = discussion.Comments.FirstOrDefault(c => c.Id == commentId);
			if (comment == null) return NotFound();

			comment.IsDeleted = true;
			return NoContent();
		}

	}
}
