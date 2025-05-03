using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Dto;
using EmocineSveikataServer.Models;
using AutoMapper;

namespace EmocineSveikataServer.Mapper
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Discussion, DiscussionDto>()
				.ForMember(dest => dest.AuthorId,
     opt => opt.MapFrom(src => src.User != null ? src.User.Id : 0))
    .ForMember(dest => dest.AuthorName,
				 opt => opt.MapFrom(src => src.User != null ? src.User.Username : null))
				.ForMember(dest => dest.AuthorPicture, 
				 opt => opt.MapFrom(src => src.User != null ? src.User.UserProfile : null));
   CreateMap<DiscussionCreateDto, Discussion>();
			CreateMap<DiscussionUpdateDto, Discussion>();
			CreateMap<Comment, CommentDto>()
				.ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));
			CreateMap<CommentCreateDto, Comment>();
			CreateMap<CommentUpdateDto, Comment>();

			// User mappings
			CreateMap<User, UserDto>();
			CreateMap<RegisterDto, User>();
		}
	}

}
