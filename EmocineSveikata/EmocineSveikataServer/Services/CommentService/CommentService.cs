using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.CommentRepository;
using EmocineSveikataServer.Services.DiscussionService;

namespace EmocineSveikataServer.Services.CommentService
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _repository;
		private readonly IDiscussionService _discussions;

		public CommentService(ICommentRepository commentRepository, IDiscussionService discussions)
		{
			_repository = commentRepository;
			_discussions = discussions;
		}

		public async Task<Discussion> AddCommentToDiscussionAsync(int discussionId, Comment comment)
		{
			var discussion = await _discussions.GetDiscussionAsync(discussionId);
			discussion.Comments.Add(comment);
			await CreateCommentAsync(comment);
			await _discussions.UpdateDiscussionAsync(discussionId, discussion);
			return discussion;
		}

		public async Task<Comment> LikeCommentAsync(int commentId)
		{
			var comment = await GetCommentAsync(commentId);
			comment.Likes++;
			await UpdateCommentAsync(commentId, comment);
			return comment;
		}

		public async Task<Comment> ReplyToCommentAsync(int discussionId, int commentId, Comment reply)
		{
			var comment = await GetCommentAsync(commentId);
			reply.DiscussionId = discussionId;
			comment.Replies.Add(reply);
			await CreateCommentAsync(reply);
			return comment;
		}

		public async Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int discussionId)
		{
			return await _repository.GetCommentsByDiscussionAsync(discussionId);
		}

		private async Task<Comment> GetCommentAsync(int commentId)
		{
			return await _repository.GetCommentAsync(commentId);
		}

		public async Task CreateCommentAsync(Comment comment)
		{
			await _repository.AddCommentAsync(comment);
		}

		public Task<Comment> UpdateCommentAsync(int commentId, Comment comment)
		{
			return _repository.UpdateCommentAsync(commentId, comment);
		}

		public async Task DeleteCommentAsync(int commentId)
		{
			await _repository.DeleteCommentAsync(commentId);
		}
	}
}
