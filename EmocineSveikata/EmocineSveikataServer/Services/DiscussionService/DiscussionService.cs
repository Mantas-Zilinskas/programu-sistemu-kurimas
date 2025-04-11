using AutoMapper;
using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Enums;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.DiscussionRepository;
using EmocineSveikataServer.Services.CommentService;
using Microsoft.IdentityModel.Tokens;

namespace EmocineSveikataServer.Services.DiscussionService
{
	public class DiscussionService : IDiscussionService
	{
		private readonly ICommentService _commentService;
		private readonly IDiscussionRepository _repository;
		private readonly IMapper _mapper;
		public DiscussionService(IDiscussionRepository repository, ICommentService commentService,
			IMapper mapper)
		{
			_repository = repository;
			_commentService = commentService;
			_mapper = mapper;
		}

		public async Task<DiscussionDto> CreateDiscussionAsync(DiscussionCreateDto discussionDto)
		{
			var discussion = _mapper.Map<Discussion>(discussionDto);
			await _repository.AddDiscussionAsync(discussion);

			return _mapper.Map<DiscussionDto>(discussion);
		}

		public async Task DeleteDiscussionAsync(int discussionId)
		{
			await _repository.DeleteDiscussionAsync(discussionId);
		}

		public async Task<List<DiscussionDto>> GetAllDiscussionsAsync()
		{
			var allDiscussions = (await _repository.GetAllDiscussionsAsync()).ToList();
			return _mapper.Map<List<DiscussionDto>>(allDiscussions);
		}

		public List<string> GetAllTags()
		{
			return Enum.GetNames(typeof(DiscussionTagEnum)).ToList();
		}

		public async Task<List<DiscussionDto>> GetPagedDiscussionsAsync(int page, int pageSize, DiscussionTagEnum? tag)
		{
			var discussions = (await _repository.GetAllDiscussionsAsync())
				.Where(d => !d.IsDeleted);

			if(tag != null)
			{
				discussions = discussions.Where(d => d.Tags != null && d.Tags.Contains(tag.Value));
			}

			var paginatedDiscussions = discussions
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			return _mapper.Map<List<DiscussionDto>>(paginatedDiscussions);
		}
		public async Task<DiscussionDto> GetDiscussionAsync(int discussionId)
		{
			var discussion = await _repository.GetDiscussionAsync(discussionId);
			FixReplies(discussion);
			return _mapper.Map<DiscussionDto>(discussion);
		}

		public async Task<DiscussionDto> UpdateDiscussionAsync(int discussionId, DiscussionUpdateDto discussionDto)
		{
			var discussion = _mapper.Map<Discussion>(discussionDto);
			var updated = await _repository.UpdateDiscussionAsync(discussionId, discussion);
			FixReplies(updated);
			return _mapper.Map<DiscussionDto>(updated);
		}
		public async Task<DiscussionDto> AddCommentToDiscussionAsync(int discussionId, CommentCreateDto commentDto)
		{
			var discussion = await _repository.GetDiscussionAsync(discussionId);
			var comment = _mapper.Map<Comment>(commentDto);
			discussion.Comments.Add(comment);
			FixReplies(discussion);
			await _commentService.CreateCommentAsync(comment);
			await _repository.SaveChangesAsync();
			return _mapper.Map<DiscussionDto>(discussion);
		}
		public async Task<DiscussionDto> AddLikeAsync(int discussionId)
		{
			var discussion = await _repository.GetDiscussionAsync(discussionId);
			discussion.Likes++;
			FixReplies(discussion);
			await SaveChangesAsync();
			return _mapper.Map<DiscussionDto>(discussion);
		}
		public async Task SaveChangesAsync()
		{
			await _repository.SaveChangesAsync();
		}

		private Discussion FixReplies(Discussion discussion)
		{
			discussion.Comments.RemoveAll(c => c.CommentId != null);
			_commentService.RemoveSoftDeletedReplies(discussion.Comments);
			return discussion;
		}

	}
}
