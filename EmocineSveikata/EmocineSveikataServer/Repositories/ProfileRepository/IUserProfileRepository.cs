using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.ProfileRepository
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetUserProfileByUserId(int userId);
        Task<UserProfile> CreateUserProfile(UserProfile userProfile);
        Task<UserProfile> UpdateUserProfile(UserProfile userProfile);
        Task<bool> UserProfileExists(int userId);
    }
}
