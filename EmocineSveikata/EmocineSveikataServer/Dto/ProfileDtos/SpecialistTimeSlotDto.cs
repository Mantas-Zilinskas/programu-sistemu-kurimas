namespace EmocineSveikataServer.Dto.ProfileDtos
{
    public class SpecialistTimeSlotDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
        public int? BookedByUserId { get; set; }
        public string? BookedByUsername { get; set; }
        public string MeetLink { get; set; } = "";
    }

    public class SpecialistTimeSlotCreateDto
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class SpecialistTimeSlotUpdateDto
    {
        public bool IsBooked { get; set; }
        public int? BookedByUserId { get; set; }
    }
}
