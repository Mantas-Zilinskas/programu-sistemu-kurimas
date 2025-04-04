using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.CommentService
{
	public interface ICommentService
	{
		Task<Discussion> AddCommentToDiscussionAsync(int discussionId, Comment comment);
		Task<Comment> LikeCommentAsync(int commentId);
		Task<Comment> ReplyToCommentAsync(int discussionId, int commentId, Comment reply);
		Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int commentId);
		Task CreateCommentAsync(Comment comment);
		Task<Comment> UpdateCommentAsync(int commentId, Comment comment);
		Task DeleteCommentAsync(int commentId);
	}
}
