using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.DiscussionService
{
	public interface IDiscussionService
	{
		Task<List<Discussion>> GetAllDiscussionsAsync();
		Task<List<Discussion>> GetPagedDiscussionsAsync(int page, int pageSize);
		Task<Discussion> GetDiscussionAsync(int discussionId);
		Task CreateDiscussionAsync(Discussion discussion);
		Task<Discussion> UpdateDiscussionAsync(int discussionId,  Discussion discussion);
		Task DeleteDiscussionAsync(int discussionId);
		Task<Discussion> AddLikeAsync(int discussionId);
		Task SaveChangesAsync();
	}
}
