namespace EmocineSveikataServer.Dto.CommentDto
{
	public class CommentDto
	{
		public int Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public int Likes { get; set; }
		public bool LikedByUser { get; set; }
		public List<CommentDto>? Replies { get; set; }
	}

}
