using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Models;
using AutoMapper;

namespace EmocineSveikataServer.Mapper
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Discussion, DiscussionDto>();
			CreateMap<DiscussionCreateDto, Discussion>();
			CreateMap<DiscussionUpdateDto, Discussion>();

			CreateMap<Comment, CommentDto>()
				.ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));
			CreateMap<CommentCreateDto, Comment>();
			CreateMap<CommentUpdateDto, Comment>();
		}
	}

}
