namespace BiteWise.DLL.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    public Guid UserEntityId { get; set; }
    public Guid ArticleId { get; set; }
    public string? Content { get; set; }
    public DateTime? Created { get; set; }
}