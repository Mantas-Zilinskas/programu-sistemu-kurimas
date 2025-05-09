using EmocineSveikataServer.Dto.CommentDtos;

namespace EmocineSveikataServer.Dto.DiscussionDisplayDto
{
  public class DiscussionDisplayDto : DiscussionDto.DiscussionDto
  {
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorPicture { get; set; }
    public new List<CommentDisplayDto>? Comments { get; set; }
  }
}
