using AutoMapper;
using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Dto.DiscussionDisplayDto;
using EmocineSveikataServer.Enums;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.DiscussionRepository;
using EmocineSveikataServer.Repositories.UserRepository;
using EmocineSveikataServer.Services.CommentService;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Xml.Linq;

namespace EmocineSveikataServer.Services.DiscussionService
{
  public class DiscussionService : IDiscussionService
  {
    private readonly ICommentService _commentService;
    private readonly IDiscussionRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public DiscussionService(IDiscussionRepository repository, ICommentService commentService,
     IUserRepository userRepository, IMapper mapper)
    {
      _repository = repository;
      _commentService = commentService;
      _userRepository = userRepository;
      _mapper = mapper;
    }

    public async Task<DiscussionDto> CreateDiscussionAsync(DiscussionCreateDto discussionDto)
    {
      var discussion = _mapper.Map<Discussion>(discussionDto);
      var user = await _userRepository.GetUserById(discussionDto.CreatorUserId);
      user.Discussions.Add(discussion);
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

    public async Task<List<DiscussionDisplayDto>> GetPagedDiscussionsAsync(int page, int pageSize, DiscussionTagEnum? tag, bool isPopular)
    {
      var paginatedDiscussions = await _repository.GetPagedDiscussionsAsync(page, pageSize, tag, isPopular);
      return _mapper.Map<List<DiscussionDisplayDto>>(paginatedDiscussions);
    }

    public async Task<DiscussionDisplayDto> GetDiscussionAsync(int discussionId, int? userId)
    {
      var discussion = await _repository.GetDiscussionWithRelationsAsync(discussionId);
      FixReplies(discussion);
      var _mapped = _mapper.Map<DiscussionDisplayDto>(discussion);

      if (userId is not null)
      {
        _mapped.LikedByUser = discussion.LikedBy.Contains((int)userId);
        return FixLikes(_mapped, discussion, (int)userId);
      }
      return _mapped;
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
      var user = await _userRepository.GetUserById(commentDto.CreatorUserId);
      var comment = _mapper.Map<Comment>(commentDto);
      discussion.Comments.Add(comment);
      user.Comments.Add(comment);
      FixReplies(discussion);
      await _commentService.CreateCommentAsync(comment);
      await _repository.SaveChangesAsync();
      return _mapper.Map<DiscussionDto>(discussion);
    }
    public async Task<DiscussionDto> ChangeLikeStatusAsync(int discussionId, int userId)
    {
      var discussion = await _repository.GetDiscussionAsync(discussionId);
      var user = await _userRepository.GetUserById(userId);

      if (!user.Discussions.Contains(discussion))
      {
        if (!discussion.LikedBy.Contains(userId))
        {
          discussion.LikedBy.Add(userId);
        }
        else
        {
          discussion.LikedBy.Remove(userId);
        }

        FixReplies(discussion);
        await SaveChangesAsync();
      }

      return FixLikes(_mapper.Map<DiscussionDto>(discussion), discussion, userId);
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

    private T FixLikes<T>(T discussionDto, Discussion discussion, int userId) where T : DiscussionDto
    {
      List<CommentDto> FixedDtos(List<CommentDto> commentsDto, List<Comment> comments)
      {
        for (int i = 0; i < commentsDto.Count; i++)
        {
          if (comments[i].LikedBy.Contains(userId))
            commentsDto[i].LikedByUser = true;

          if (commentsDto[i].Replies is not null)
            FixedDtos(commentsDto[i].Replies, comments[i].Replies);
        }
        return commentsDto;
      }

      if (discussionDto.Comments is not null)
        FixedDtos(discussionDto.Comments, discussion.Comments);

      discussionDto.LikedByUser = discussion.LikedBy.Contains(userId);
      return discussionDto;
    }
  }
}
