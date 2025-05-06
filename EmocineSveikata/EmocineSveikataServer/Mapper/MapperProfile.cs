using EmocineSveikataServer.Dto.CommentDto;
using EmocineSveikataServer.Dto.DiscussionDto;
using EmocineSveikataServer.Dto;
using EmocineSveikataServer.Models;
using AutoMapper;
using EmocineSveikataServer.Dto.DiscussionDisplayDto;
using EmocineSveikataServer.Dto.CommentDtos;

namespace EmocineSveikataServer.Mapper
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
   CreateMap<Discussion, DiscussionDisplayDto>()
    .ForMember(dest => dest.AuthorId,
     opt => opt.MapFrom(src => src.User != null ? src.User.Id : 0))
    .ForMember(dest => dest.AuthorName,
     opt => opt.MapFrom(src => src.User != null ? src.User.Username : null))
    .ForMember(dest => dest.AuthorPicture,
     opt => opt.MapFrom(src => src.User != null ? src.User.UserProfile.ProfilePicture : null));
   CreateMap<Discussion, DiscussionDto>();
   CreateMap<DiscussionCreateDto, Discussion>();
			CreateMap<DiscussionUpdateDto, Discussion>();
			CreateMap<Comment, CommentDto>()
				.ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));
			CreateMap<Comment, CommentDisplayDto>()
    .ForMember(dest => dest.AuthorId,
     opt => opt.MapFrom(src => src.User != null ? src.User.Id : 0))
    .ForMember(dest => dest.AuthorName,
     opt => opt.MapFrom(src => src.User != null ? src.User.Username : null))
    .ForMember(dest => dest.AuthorPicture,
     opt => opt.MapFrom(src => src.User != null ? src.User.UserProfile.ProfilePicture : null));
      CreateMap<CommentCreateDto, Comment>();
			CreateMap<CommentUpdateDto, Comment>();

			// User mappings
			CreateMap<User, UserDto>();
			CreateMap<RegisterDto, User>();
		}
	}

}
