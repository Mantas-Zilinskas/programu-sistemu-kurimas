using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EmocineSveikataServer.Repositories.DiscussionRepository
{
	public class DiscussionRepository : IDiscussionRepository
	{
		private readonly DataContext _context;
		public DiscussionRepository(DataContext context)
		{
			_context = context;
		}

		public async Task AddDiscussionAsync(Discussion discussion)
		{
			_context.Discussions.Add(discussion);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteDiscussionAsync(int id)
		{
			var discussion = await GetDiscussionAsync(id);
			discussion.IsDeleted = true;
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Discussion>> GetAllDiscussionsAsync()
		{
			return await _context.Discussions.OrderByDescending(d => d.Id).ToListAsync();
		}

		public async Task<Discussion> GetDiscussionAsync(int id)
		{
			var discussion = await _context.Discussions
				.Include(d => d.Comments)
				.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

			if (discussion is null)
			{
				throw new KeyNotFoundException("Discussion not found");
			}

			discussion.Comments.RemoveAll(c => c.IsDeleted);
			return discussion;
		}

		public async Task<Discussion> UpdateDiscussionAsync(int id, Discussion discussion)
		{
			var existing = await GetDiscussionAsync(id);
			existing.Title = discussion.Title;
			existing.Content = discussion.Content;
			existing.Tags = discussion.Tags;

			await _context.SaveChangesAsync();
			return existing;
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
