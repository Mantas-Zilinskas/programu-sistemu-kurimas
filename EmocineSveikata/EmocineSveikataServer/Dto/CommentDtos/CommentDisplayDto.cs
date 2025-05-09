namespace EmocineSveikataServer.Dto.CommentDtos
{
  public class CommentDisplayDto : CommentDto.CommentDto
  {
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorPicture { get; set; }
    public List<CommentDisplayDto>? Replies { get; set; } = [];
  }
}
