using System.ComponentModel.DataAnnotations;

namespace EmocineSveikataServer.Dto.ProfileDtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public List<string>? SelectedTopics { get; set; }
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        public bool EnableSmsNotifications { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }

    public class UserProfileUpdateDto
    {
        public string? ProfilePicture { get; set; }
        public List<string>? SelectedTopics { get; set; }
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        public bool EnableSmsNotifications { get; set; }
    }
}
