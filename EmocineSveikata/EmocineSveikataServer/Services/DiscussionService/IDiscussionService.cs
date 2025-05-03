using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Enums;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.DiscussionService
{
	public interface IDiscussionService
	{
		Task<List<DiscussionDto>> GetAllDiscussionsAsync();
		List<string> GetAllTags();
		Task<List<DiscussionDto>> GetPagedDiscussionsAsync(int page, int pageSize, DiscussionTagEnum? tag, bool isPopular);
		Task<DiscussionDto> GetDiscussionAsync(int discussionId, int? userId);
		Task<DiscussionDto> CreateDiscussionAsync(DiscussionCreateDto discussion);
		Task<DiscussionDto> UpdateDiscussionAsync(int discussionId,  DiscussionUpdateDto discussion);
		Task<DiscussionDto> AddCommentToDiscussionAsync(int discussionId, CommentCreateDto commentDto);
		Task DeleteDiscussionAsync(int discussionId);
		Task<DiscussionDto> ChangeLikeStatusAsync(int discussionId, int userId);
		Task SaveChangesAsync();
	}
}
