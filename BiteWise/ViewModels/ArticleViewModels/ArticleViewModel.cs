using BiteWise.BLL.Models;

namespace BiteWise.ViewModels.ArticleViewModels;

public class ArticleViewModel
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? UserEntityId { get; set; }
    public string? Image { get; set; }
    public required DateTime Created { get; set; }
    public List<Tag>? Tags { get; set; }
}