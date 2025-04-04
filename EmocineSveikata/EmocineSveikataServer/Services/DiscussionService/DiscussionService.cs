using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.DiscussionRepository;

namespace EmocineSveikataServer.Services.DiscussionService
{
	public class DiscussionService : IDiscussionService
	{
		private readonly IDiscussionRepository _repository;
		public DiscussionService(IDiscussionRepository repository)
		{
			_repository = repository;
		}

		public async Task CreateDiscussionAsync(Discussion discussion)
		{
			await _repository.AddDiscussionAsync(discussion);
		}

		public async Task DeleteDiscussionAsync(int discussionId)
		{
			await _repository.DeleteDiscussionAsync(discussionId);
		}

		public async Task<List<Discussion>> GetAllDiscussionsAsync()
		{
			return (await _repository.GetAllDiscussionsAsync()).ToList();
		}
		public async Task<List<Discussion>> GetPagedDiscussionsAsync(int page, int pageSize)
		{
			var paginatedDiscussions = (await _repository.GetAllDiscussionsAsync()).Where(d => !d.IsDeleted)
										.Skip((page - 1) * pageSize)
										.Take(pageSize)
										.ToList();
			return paginatedDiscussions;
		}
		public async Task<Discussion> GetDiscussionAsync(int discussionId)
		{
			var discussion = await _repository.GetDiscussionAsync(discussionId);
			
			return discussion;
		}

		public async Task<Discussion> UpdateDiscussionAsync(int discussionId, Discussion discussion)
		{
			return await _repository.UpdateDiscussionAsync(discussionId, discussion);
		}

		public async Task<Discussion> AddLikeAsync(int discussionId)
		{
			var discussion = await GetDiscussionAsync(discussionId);
			discussion.Likes++;
			return await UpdateDiscussionAsync(discussionId, discussion);
		}

		public async Task SaveChangesAsync()
		{
			await _repository.SaveChangesAsync();
		}
	}
}
