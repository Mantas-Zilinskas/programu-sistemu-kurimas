using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Repositories.ProfileRepository
{
    public class SpecialistProfileRepository : ISpecialistProfileRepository
    {
        private readonly DataContext _context;

        public SpecialistProfileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<SpecialistProfile?> GetSpecialistProfileByUserId(int userId)
        {
            return await _context.SpecialistProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<SpecialistProfile> CreateSpecialistProfile(SpecialistProfile specialistProfile)
        {
            _context.SpecialistProfiles.Add(specialistProfile);
            await _context.SaveChangesAsync();
            return specialistProfile;
        }

        public async Task<SpecialistProfile> UpdateSpecialistProfile(SpecialistProfile specialistProfile)
        {
            specialistProfile.UpdatedAt = DateTime.UtcNow;
            _context.SpecialistProfiles.Update(specialistProfile);
            await _context.SaveChangesAsync();
            return specialistProfile;
        }

        public async Task<bool> SpecialistProfileExists(int userId)
        {
            return await _context.SpecialistProfiles.AnyAsync(p => p.UserId == userId);
        }
    }
}
