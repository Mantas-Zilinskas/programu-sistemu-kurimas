using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.ProfileRepository
{
    public interface ISpecialistProfileRepository
    {
        Task<SpecialistProfile?> GetSpecialistProfileByUserId(int userId);
        Task<List<SpecialistProfile>> GetSpecialistProfilesByUserIds(List<int> userIds);
        Task<SpecialistProfile> CreateSpecialistProfile(SpecialistProfile specialistProfile);
        Task<SpecialistProfile> UpdateSpecialistProfile(SpecialistProfile specialistProfile);
        Task<bool> SpecialistProfileExists(int userId);
    }
}
