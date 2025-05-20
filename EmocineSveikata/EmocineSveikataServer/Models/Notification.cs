namespace EmocineSveikataServer.Models
{
	public class Notification
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public required string Message { get; set; }
		public string? Link { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; } = false;
	}
}