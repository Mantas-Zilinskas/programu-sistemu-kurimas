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
			.Where(c => !c.IsDeleted && c.Replies != null)
			.ToListAsync();
	}

	public async Task<Comment> GetCommentAsync(int id)
	{
		var comment = await _context.Comments.FindAsync(id);
		if (comment is null)
		{
			throw new ArgumentException("Comment not found");
		}

		return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
	}

	public async Task AddCommentAsync(Comment comment)
	{
		_context.Comments.Add(comment);
		await _context.SaveChangesAsync();
	}

	public async Task<Comment> UpdateCommentAsync(int id, Comment comment)
	{
		if (id != comment.Id)
		{
			throw new ArgumentException("Comment not found");
		}

		_context.Entry(await GetCommentAsync(id)).CurrentValues.SetValues(comment);

		await _context.SaveChangesAsync();
		return comment;
	}

	public async Task DeleteCommentAsync(int commentId)
	{
		var comment = await GetCommentAsync(commentId);
		if (comment != null)
		{
			comment.IsDeleted = true;
			await _context.SaveChangesAsync();
		}
	}
}
