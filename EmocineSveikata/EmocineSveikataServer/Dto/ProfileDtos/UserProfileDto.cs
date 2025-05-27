namespace EmocineSveikataServer.Dto.ProfileDtos
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public List<string>? SelectedTopics { get; set; }
        public bool SmsNotificationsEnabled { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SmsReminderTopic { get; set; }
        public DateTime? LastSmsReminder { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UserProfileUpdateDto
    {
        public string? ProfilePicture { get; set; }
        public List<string>? SelectedTopics { get; set; }
        public bool SmsNotificationsEnabled { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SmsReminderTopic { get; set; }
    }
}
