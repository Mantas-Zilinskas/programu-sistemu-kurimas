﻿using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Dto.DiscussionDisplayDto;
using EmocineSveikataServer.Enums;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Services.DiscussionService
{
	public interface IDiscussionService
	{
		Task<List<DiscussionDto>> GetAllDiscussionsAsync();
		List<string> GetAllTags();
		Task<List<DiscussionDisplayDto>> GetPagedDiscussionsAsync(int page, int pageSize, DiscussionTagEnum? tag, bool isPopular);
		Task<DiscussionDisplayDto> GetDiscussionAsync(int discussionId, int? userId);
		Task<DiscussionDto> CreateDiscussionAsync(DiscussionCreateDto discussion);
		Task<DiscussionDto> ForceUpdateDiscussionAsync(int discussionId, DiscussionUpdateDto discussion);
		Task<DiscussionDto> UpdateDiscussionAsync(int discussionId,  DiscussionUpdateDto discussion);
		Task<DiscussionDisplayDto> AddCommentToDiscussionAsync(int discussionId, CommentCreateDto commentDto);
		Task DeleteDiscussionAsync(int discussionId);
		Task<DiscussionDto> ChangeLikeStatusAsync(int discussionId, int userId);
		Task SaveChangesAsync();
	}
}
