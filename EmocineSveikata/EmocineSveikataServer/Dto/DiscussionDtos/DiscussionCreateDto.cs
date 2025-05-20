using EmocineSveikataServer.Enums;

namespace EmocineSveikataServer.Dto.DiscussionDto
{
	public class DiscussionCreateDto
	{
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public List<DiscussionTagEnum>? Tags { get; set; }
		public int CreatorUserId { get; set; }
	}

}
