namespace BiteWise.BLL.Models;

public class Article
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? UserEntityId { get; set; }
    public string? Image { get; set; }
    public required DateTime Created { get; set; }
}