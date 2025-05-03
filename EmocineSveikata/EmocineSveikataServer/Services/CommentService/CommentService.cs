using AutoMapper;
using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.CommentRepository;
using EmocineSveikataServer.Repositories.UserRepository;
using Microsoft.IdentityModel.Tokens;

namespace EmocineSveikataServer.Services.CommentService
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _repository;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;

		public CommentService(ICommentRepository commentRepository, IUserRepository userRepository,
			IMapper mapper)
		{
			_repository = commentRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<CommentDto> ChangeLikeStatusCommentAsync(int commentId, int userId)
		{
			var comment = await _repository.GetCommentAsync(commentId);
			var user = await _userRepository.GetUserById(userId);

			if (!user.Comments.Contains(comment))
			{
				if (!comment.LikedBy.Contains(userId))
				{
					comment.LikedBy.Add(userId);
				}
				else
				{
					comment.LikedBy.Remove(userId);
				}
			}

			await SaveChangesAsync();
			var _mapped = _mapper.Map<CommentDto>(comment);
			_mapped.LikedByUser = comment.LikedBy.Contains(userId);
			return _mapped;
		}

		public async Task<CommentDto> ReplyToCommentAsync(int discussionId, int commentId, CommentCreateDto replyDto, int userId)
		{
			var comment = await _repository.GetCommentAsync(commentId);
			var reply = _mapper.Map<Comment>(replyDto);
			var user = await _userRepository.GetUserById(userId);
			user.Comments.Add(reply);
			reply.DiscussionId = discussionId;
			comment.Replies.Add(reply);
			await CreateCommentAsync(reply);
			return _mapper.Map<CommentDto>(comment);
		}

		public async Task<IEnumerable<Comment>> GetCommentsByDiscussionAsync(int discussionId)
		{
			return await _repository.GetCommentsByDiscussionAsync(discussionId);
		}

		public async Task CreateCommentAsync(Comment comment)
		{
			await _repository.AddCommentAsync(comment);
		}

		public async Task<CommentDto> UpdateCommentAsync(int commentId, CommentUpdateDto commentDto)
		{
			var comment = _mapper.Map<Comment>(commentDto);
			var updatedComment = await _repository.UpdateCommentAsync(commentId, comment);
			return _mapper.Map<CommentDto>(updatedComment);
		}

		public async Task DeleteCommentAsync(int commentId)
		{
			await _repository.DeleteCommentAsync(commentId);
		}

		public async Task SaveChangesAsync()
		{
			await _repository.SaveChangesAsync();
		}

		public List<Comment> RemoveSoftDeletedReplies(List<Comment> comments)
		{
			foreach (var comment in comments)
			{
				if (!comment.Replies.IsNullOrEmpty())
				{
					RemoveSoftDeletedReplies(comment.Replies);
				}
			}
			comments.RemoveAll(c => c.IsDeleted);
			return comments;
		}
	}
}
