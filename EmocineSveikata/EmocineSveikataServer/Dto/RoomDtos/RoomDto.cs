namespace EmocineSveikataServer.Dto.RoomDtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int SpecialistId { get; set; }
        public string? SpecialistName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
