using EmocineSveikataServer.Models;

namespace EmocineSveikataServer.Repositories.ProfileRepository
{
    public interface ISpecialistTimeSlotRepository
    {
        Task<List<SpecialistTimeSlot>> GetCurrentlyAvailableTimeSlots();
        Task<List<SpecialistTimeSlot>> GetTimeSlotsBySpecialistId(int specialistId);
        Task<List<SpecialistTimeSlot>> GetTimeSlotsByDateRange(int specialistId, DateTime startDate, DateTime endDate);
        Task<SpecialistTimeSlot?> GetTimeSlotById(int timeSlotId);
        Task<SpecialistTimeSlot> CreateTimeSlot(SpecialistTimeSlot timeSlot);
        Task<SpecialistTimeSlot> UpdateTimeSlot(SpecialistTimeSlot timeSlot);
        Task<bool> DeleteTimeSlot(int timeSlotId);
    }
}
