using BiteWise.BLL.Models;

namespace BiteWise.Contracts.ArticleDtos;

public class ArticleDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? UserEntityId { get; set; }
    public string? Image { get; set; }
    public required DateTime Created { get; set; }
    public List<Tag>? Tags { get; set; }
    public List<Comment>? Comments { get; set; }
}