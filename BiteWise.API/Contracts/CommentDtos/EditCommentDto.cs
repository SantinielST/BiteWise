namespace BiteWise.Contracts.CommentDto;

public class EditCommentDto
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? Content { get; set; }
    public required Guid ArticleId { get; set; }
}