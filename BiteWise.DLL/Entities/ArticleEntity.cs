namespace BiteWise.DLL.Entities;

public class ArticleEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Guid UserEntityId { get; set; }
}