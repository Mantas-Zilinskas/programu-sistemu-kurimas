using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveChanges();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetUsersByIds(List<int> ids) 
        {
            var usersDict = await _context.Users
                .Where(user => ids.Contains(user.Id))
                .ToDictionaryAsync(u => u.Id);

            var result = ids
                .Where(id => usersDict.ContainsKey(id))
                .Select(id => usersDict[id])
                .ToList();

            return result;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<UserProfile?> GetUserProfileAsync(int userId)
        {
            return await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
        
        public async Task<List<UserProfile>> GetAllUserProfilesWithSmsEnabled()
        {
            return await _context.UserProfiles
                .Where(p => p.EnableSmsNotifications && !string.IsNullOrEmpty(p.PhoneNumber))
                .ToListAsync();
        }

        public async Task<UserProfile> UpdateUserProfileAsync(UserProfile profile)
        {
            var existingProfile = await _context.UserProfiles.FindAsync(profile.Id);
            
            if (existingProfile == null)
            {
                await _context.UserProfiles.AddAsync(profile);
            }
            else
            {
                _context.Entry(existingProfile).CurrentValues.SetValues(profile);
                existingProfile.UpdatedAt = DateTime.UtcNow;
            }
            
            await SaveChanges();
            return profile;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
