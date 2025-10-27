namespace BiteWise.BLL.Models;

public class Tag
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string ArticleId { get; set; }
    public string? Link { get; set; }
    public DateTime? Created { get; set; }
}