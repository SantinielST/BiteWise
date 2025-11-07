namespace BiteWise.BLL.Models;

public class Comment
{
    public Guid Id { get; set; }
    public required Guid UserEntityId { get; set; }
    public required Guid ArticleId { get; set; }
    public string? Content { get; set; }
    public DateTime? Created { get; set; }
}