using System.ComponentModel.DataAnnotations;

namespace EmocineSveikataServer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public byte[] PasswordHash { get; set; } = new byte[0];
        
        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[0];
        
        [Required]
        public string Role { get; set; } = "Naudotojas"; // Default role yra "Naudotojas"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
    }
}
