using EmocineSveikataServer.Enums;

namespace EmocineSveikataServer.Dto.DiscussionDto
{
	public class DiscussionUpdateDto
	{
		public string? Title { get; set; }
		public string? Content { get; set; }
		public List<DiscussionTagEnum>? Tags { get; set; }
		public required byte[] RowVersion { get; set; }
	}
}
