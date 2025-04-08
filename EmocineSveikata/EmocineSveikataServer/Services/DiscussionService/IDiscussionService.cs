using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.DiscussionService
{
	public interface IDiscussionService
	{
		Task<List<DiscussionDto>> GetAllDiscussionsAsync();
		Task<List<DiscussionDto>> GetPagedDiscussionsAsync(int page, int pageSize);
		Task<DiscussionDto> GetDiscussionAsync(int discussionId);
		Task<DiscussionDto> CreateDiscussionAsync(DiscussionCreateDto discussion);
		Task<DiscussionDto> UpdateDiscussionAsync(int discussionId,  DiscussionUpdateDto discussion);
		Task<DiscussionDto> AddCommentToDiscussionAsync(int discussionId, CommentCreateDto commentDto);
		Task DeleteDiscussionAsync(int discussionId);
		Task<DiscussionDto> AddLikeAsync(int discussionId);
		Task SaveChangesAsync();
	}
}
