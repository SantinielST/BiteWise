namespace BiteWise.BLL.Models;

public class Comment
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string ArticleId { get; set; }
    public string? Content { get; set; }
    public DateTime? Created { get; set; }
}