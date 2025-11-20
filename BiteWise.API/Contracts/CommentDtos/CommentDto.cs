namespace BiteWise.Contracts.CommentDto;

public class CommentDto
{
    public Guid UserId { get; set; }
    public  Guid ArticleId { get; set; }
    public string? Content { get; set; }
}