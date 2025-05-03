using System.ComponentModel.DataAnnotations.Schema;

namespace EmocineSveikataServer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int? DiscussionId { get; set; }
        public int? CommentId { get; set; }
        public required string Content { get; set; }
        public List<int> LikedBy { get; set; } = new List<int>();
        [NotMapped]
        public int Likes => LikedBy.Count;
        public bool IsDeleted { get; set; }
        public List<Comment> Replies { get; set; } = new List<Comment>();
    }
}
