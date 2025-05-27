using EmocineSveikataServer.Data;
using EmocineSveikataServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EmocineSveikataServer.Repositories.ProfileRepository
{
    public class SpecialistTimeSlotRepository : ISpecialistTimeSlotRepository
    {
        private readonly DataContext _context;

        public SpecialistTimeSlotRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SpecialistTimeSlot>> GetCurrentlyAvailableTimeSlots()
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime currentDate = currentDateTime.Date;
            TimeSpan currentTime = currentDateTime.TimeOfDay;

            return await _context.SpecialistTimeSlots
                .Where(ts =>
                    ts.Date > currentDate || // Future dates
                    (ts.Date == currentDate && ts.EndTime > currentTime) // Today and not yet ended
                )
                .Where(ts => !ts.IsBooked)
                .OrderBy(ts => ts.Date)
                .ThenBy(ts => ts.StartTime)
                .ToListAsync();
        }

        public async Task<List<SpecialistTimeSlot>> GetTimeSlotsBySpecialistId(int specialistId)
        {
            return await _context.SpecialistTimeSlots
                .Where(ts => ts.UserId == specialistId)
                .OrderBy(ts => ts.Date)
                .ThenBy(ts => ts.StartTime)
                .ToListAsync();
        }

        public async Task<List<SpecialistTimeSlot>> GetTimeSlotsByDateRange(int specialistId, DateTime startDate, DateTime endDate)
        {
            return await _context.SpecialistTimeSlots
                .Where(ts => ts.UserId == specialistId && ts.Date >= startDate && ts.Date <= endDate)
                .OrderBy(ts => ts.Date)
                .ThenBy(ts => ts.StartTime)
                .ToListAsync();
        }

		public async Task<List<SpecialistTimeSlot>> GetBookedTimeSlotsByUserId(int userId)
		{
			return await _context.SpecialistTimeSlots
				.Where(ts => ts.BookedByUserId == userId && ts.IsBooked)
				.Include(ts => ts.User)
				.Include(ts => ts.BookedByUser)
				.OrderBy(ts => ts.Date)
				.ThenBy(ts => ts.StartTime)
				.ToListAsync();
		}

		public async Task<SpecialistTimeSlot?> GetTimeSlotById(int timeSlotId)
        {
            return await _context.SpecialistTimeSlots
                .FirstOrDefaultAsync(ts => ts.Id == timeSlotId);
        }

        public async Task<SpecialistTimeSlot> CreateTimeSlot(SpecialistTimeSlot timeSlot)
        {
            _context.SpecialistTimeSlots.Add(timeSlot);
            await _context.SaveChangesAsync();
            return timeSlot;
        }

        public async Task<SpecialistTimeSlot> UpdateTimeSlot(SpecialistTimeSlot timeSlot)
        {
            _context.SpecialistTimeSlots.Update(timeSlot);
            await _context.SaveChangesAsync();
            return timeSlot;
        }

        public async Task<bool> DeleteTimeSlot(int timeSlotId)
        {
            var timeSlot = await _context.SpecialistTimeSlots.FindAsync(timeSlotId);
            if (timeSlot == null)
            {
                return false;
            }

            _context.SpecialistTimeSlots.Remove(timeSlot);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
