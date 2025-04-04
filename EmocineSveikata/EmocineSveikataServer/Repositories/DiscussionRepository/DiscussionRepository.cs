using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;

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
			if (discussion is not null)
			{
				discussion.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new ArgumentException("Discussion not found");
			}
		}

		public async Task<IEnumerable<Discussion>> GetAllDiscussionsAsync()
		{
			return await _context.Discussions.ToListAsync();
		}

		public async Task<Discussion> GetDiscussionAsync(int id)
		{
			var discussion = await _context.Discussions.FindAsync(id);
			if (discussion is null)
			{
				throw new ArgumentException("Discussion not found");
			}

			return await _context.Discussions.Include(d => d.Comments).FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
		}

		public async Task<Discussion> UpdateDiscussionAsync(int id, Discussion discussion)
		{
			if (id != discussion.Id)
			{
				throw new ArgumentException("Discussion not found");
			}

			_context.Entry(await GetDiscussionAsync(id)).CurrentValues.SetValues(discussion);

			await _context.SaveChangesAsync();
			return discussion;
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
