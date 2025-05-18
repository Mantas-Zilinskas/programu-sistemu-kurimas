using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.CommentDtos;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.CommentService
{
	public interface ICommentService
	{
		Task<CommentDto> ChangeLikeStatusCommentAsync(int commentId, int userId);
		Task<CommentDisplayDto> ReplyToCommentAsync(int discussionId, int commentId, CommentCreateDto replyDto, int userId);
		Task<CommentDto> UpdateCommentAsync(int commentId, CommentUpdateDto commentDto);
		Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int discussionId);
		Task CreateCommentAsync(Comment comment);
		Task DeleteCommentAsync(int commentId);
		Task SaveChangesAsync();
		List<Comment> RemoveSoftDeletedReplies(List<Comment> comments);
	}
}
