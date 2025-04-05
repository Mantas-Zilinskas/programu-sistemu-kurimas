namespace EmocineSveikataServer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int? DiscussionId { get; set; }
        public int? CommentId { get; set; }
        public required string Content { get; set; }
        public int Likes { get; set; }
        public bool IsDeleted { get; set; }
        public List<Comment>? Replies { get; set; } = new List<Comment>();
    }
}
