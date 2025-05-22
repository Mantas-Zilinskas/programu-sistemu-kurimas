namespace EmocineSveikataServer.Dto
{
	public class NotificationDto
	{
		public int Id { get; set; }
		public required string Message { get; set; }
		public string? Link { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; } = false;
	}
}