using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.CommentService
{
	public interface ICommentService
	{
		Task<CommentDto> LikeCommentAsync(int commentId);
		Task<CommentDto> ReplyToCommentAsync(int discussionId, int commentId, CommentCreateDto replyDto);
		Task<CommentDto> UpdateCommentAsync(int commentId, CommentUpdateDto commentDto);
		Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int discussionId);
		Task CreateCommentAsync(Comment comment);
		Task DeleteCommentAsync(int commentId);
		Task SaveChangesAsync();
		List<Comment> RemoveSoftDeletedReplies(List<Comment> comments);
	}
}
