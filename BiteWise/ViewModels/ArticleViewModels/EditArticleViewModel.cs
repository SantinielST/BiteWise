namespace BiteWise.ViewModels.ArticleViewModels;

public class EditArticleViewModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? UserEntityId { get; set; }
    public string? Image { get; set; }

    public string? ReturnUrl { get; set; }
}