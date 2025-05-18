using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Repositories.ProfileRepository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly DataContext _context;

        public UserProfileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetUserProfileByUserId(int userId)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<UserProfile> CreateUserProfile(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
            return userProfile;
        }

        public async Task<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            userProfile.UpdatedAt = DateTime.UtcNow;
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();
            return userProfile;
        }

        public async Task<bool> UserProfileExists(int userId)
        {
            return await _context.UserProfiles.AnyAsync(p => p.UserId == userId);
        }
    }
}
