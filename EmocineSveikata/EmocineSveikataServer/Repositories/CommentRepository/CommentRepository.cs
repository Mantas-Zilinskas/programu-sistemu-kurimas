using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.CommentRepository;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : ICommentRepository
{
	private readonly DataContext _context;

	public CommentRepository(DataContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int discussionId)
	{
		return await _context.Comments
			.Where(c => !c.IsDeleted && c.DiscussionId == discussionId)
			.ToListAsync();
	}

	public async Task<Comment> GetCommentAsync(int id)
	{
		var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
		if (comment is null)
		{
			throw new KeyNotFoundException("Comment not found");
		}

		return comment;
	}

	public async Task<Comment> GetCommentWithRelationsAsync(int id)
	{
		var comment = await _context
			.Comments
			.Include(c => c.User)
			.ThenInclude(u => u.UserProfile)
			.Include(c => c.Replies)
			.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
		if (comment is null)
		{
			throw new KeyNotFoundException("Comment not found");
		}

		return comment;
	}

	public async Task AddCommentAsync(Comment comment)
	{
		_context.Comments.Add(comment);
		await _context.SaveChangesAsync();
	}

	public async Task<Comment> UpdateCommentAsync(int id, Comment comment)
	{
		var existing = await GetCommentAsync(id);
		existing.Content = comment.Content;
		await _context.SaveChangesAsync();
		return existing;
	}

	public async Task DeleteCommentAsync(int commentId)
	{
		var comment = await GetCommentAsync(commentId);
		comment.IsDeleted = true;
		await _context.SaveChangesAsync();
	}
	
	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}
}
