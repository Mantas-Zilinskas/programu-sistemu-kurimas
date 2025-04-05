using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.CommentRepository
{
	public interface ICommentRepository
	{
		Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int discussionId);
		Task<Comment> GetCommentAsync(int commentId);
		Task AddCommentAsync(Comment comment);
		Task<Comment> UpdateCommentAsync(int commentId,  Comment comment);
		Task DeleteCommentAsync(int commentId);
		Task SaveChangesAsync();
	}
}
