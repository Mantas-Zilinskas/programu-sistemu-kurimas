using EmocineSveikataServer.Enums;

namespace EmocineSveikataServer.Models
{
    public class DiscussionModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public int Likes { get; set; }
        public bool IsDeleted { get; set; }
        public List<DiscussionTagEnum>? Tags { get; set; } = new List<DiscussionTagEnum>();
        public List<CommentModel>? Comments { get; set; } = new List<CommentModel>();
    }
}
