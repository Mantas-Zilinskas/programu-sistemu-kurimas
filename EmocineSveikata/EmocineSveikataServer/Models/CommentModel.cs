namespace EmocineSveikataServer.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public int Likes { get; set; }
        public bool IsDeleted { get; set; }
        public List<CommentModel>? Replies { get; set; } = new List<CommentModel>();
    }
}
