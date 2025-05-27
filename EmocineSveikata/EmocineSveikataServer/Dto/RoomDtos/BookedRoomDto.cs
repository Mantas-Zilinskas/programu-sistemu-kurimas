namespace EmocineSveikataServer.Dto.RoomDtos
{
	public class BookedRoomDto
	{
		public int Id { get; set; }
		public string? SpecialistName { get; set; }
		public string? ProfilePicture { get; set; }
		public string? Bio { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public DateTime BookedAt { get; set; }
		public string MeetLink { get; set; }
	}
}
