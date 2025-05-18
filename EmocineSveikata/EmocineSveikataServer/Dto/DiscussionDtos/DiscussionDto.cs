using EmocineSveikataServer.Enums;

namespace EmocineSveikataServer.Dto.DiscussionDto
{
	public class DiscussionDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public int Likes { get; set; }
		public bool LikedByUser { get; set; }
		public List<DiscussionTagEnum>? Tags { get; set; }
		public List<CommentDto.CommentDto>? Comments { get; set; }
		public byte[] RowVersion { get; set; }
	}
}
