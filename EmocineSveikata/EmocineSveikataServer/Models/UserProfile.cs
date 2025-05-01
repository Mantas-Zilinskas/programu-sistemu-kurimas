using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmocineSveikataServer.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        public string? ProfilePicture { get; set; }
        
        public string? SelectedTopics { get; set; } // Stored as JSON string
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
