using EmocineSveikataServer.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmocineSveikataServer.Models
{
    public class Discussion
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public List<int> LikedBy { get; set; } = new List<int>();
        [NotMapped]
        public int Likes => LikedBy.Count;
        public bool IsDeleted { get; set; }
        public List<DiscussionTagEnum> Tags { get; set; } = new List<DiscussionTagEnum>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
