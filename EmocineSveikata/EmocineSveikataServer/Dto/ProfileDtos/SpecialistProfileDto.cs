namespace EmocineSveikataServer.Dto.ProfileDtos
{
    public class SpecialistProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public List<string>? SelectedTopics { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SpecialistProfileUpdateDto
    {
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public List<string>? SelectedTopics { get; set; }
    }
}
