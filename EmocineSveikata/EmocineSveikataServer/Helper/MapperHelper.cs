using AutoMapper;
using EmocineSveikataServer.Dto.DiscussionDisplayDto;
using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Helper
{
  public class MapperHelper
  {
    public static string? GetProfilePicture(User user)
    {
      return !string.IsNullOrEmpty(user.UserProfile?.ProfilePicture)
          ? user.UserProfile.ProfilePicture
          : user.SpecialistProfile?.ProfilePicture;
    }
  }
}