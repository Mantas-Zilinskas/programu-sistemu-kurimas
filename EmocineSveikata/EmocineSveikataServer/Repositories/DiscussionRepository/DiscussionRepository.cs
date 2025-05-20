using EmocineSveikataServer.Data;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Enums;
using EmocineSveikataServer.Migrations;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
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

		public async Task<IEnumerable<Discussion>> GetPagedDiscussionsAsync(int page, int pageSize, DiscussionTagEnum? tag)
		{
			var query = _context.Discussions
			 .Where(d => !d.IsDeleted);

			if (tag != null)
				query = query.Where(d => d.Tags != null && d.Tags.Contains(tag.Value));

			query = query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Include(d => d.User).ThenInclude(u => u.UserProfile)
			.Include(d => d.User).ThenInclude(u => u.SpecialistProfile);


			query = query.OrderByDescending(d => d.Id);

			return await query.ToListAsync();
		}

		public async Task<Discussion> GetDiscussionAsync(int id)
		{
			var discussion = await _context.Discussions
			 .Include(d => d.Comments)
			 .Include(d => d.User)
			 .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

			if (discussion is null)
			{
				throw new KeyNotFoundException("Discussion not found");
			}

			discussion.Comments.RemoveAll(c => c.IsDeleted);
			return discussion;
		}

		public async Task<Discussion> GetDiscussionWithRelationsAsync(int id)
		{
			var discussion = await _context.Discussions
				.Include(d => d.Comments).ThenInclude(c => c.User).ThenInclude(u => u.UserProfile)
				.Include(d => d.Comments).ThenInclude(c => c.User).ThenInclude(u => u.SpecialistProfile)
				.Include(d => d.Comments).ThenInclude(c => c.Replies)
				.Include(d => d.User).ThenInclude(u => u.UserProfile)
				.Include(d => d.User).ThenInclude(u => u.SpecialistProfile)
				.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

			if (discussion is null)
			{
				throw new KeyNotFoundException("Discussion not found");
			}

			discussion.Comments.RemoveAll(c => c.IsDeleted);
			return discussion;
		}

		public async Task<Discussion> ForceUpdateDiscussionAsync(int id, Discussion discussion)
		{
			var existing = await GetDiscussionAsync(id);
			existing.Title = discussion.Title;
			existing.Content = discussion.Content;
			existing.Tags = discussion.Tags;

			await _context.SaveChangesAsync();
			return existing;
		}

		public async Task<Discussion> UpdateDiscussionAsync(int id, Discussion discussion)
		{
			var existing = await GetDiscussionAsync(id);
			existing.Title = discussion.Title;
			existing.Content = discussion.Content;
			existing.Tags = discussion.Tags;

			_context.Entry(existing).Property(d => d.RowVersion).OriginalValue = discussion.RowVersion;

			await _context.SaveChangesAsync();
			try
			{
				await _context.SaveChangesAsync();
				return existing;
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new InvalidOperationException("Concurrency conflict: the discussion was updated or deleted by another user.");
			}
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
