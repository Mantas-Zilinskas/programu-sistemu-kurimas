using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.DiscussionRepository
{
	public interface IDiscussionRepository
	{
		Task<IEnumerable<Discussion>> GetAllDiscussionsAsync();
		Task<Discussion> GetDiscussionAsync(int id);
		Task AddDiscussionAsync(Discussion discussion);
		Task<Discussion> UpdateDiscussionAsync(int id, Discussion discussion);
		Task DeleteDiscussionAsync(int id);

		Task SaveChangesAsync();
	}
}
