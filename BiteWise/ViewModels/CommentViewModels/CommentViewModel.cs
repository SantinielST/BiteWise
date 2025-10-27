namespace BiteWise.ViewModels.CommentViewModels;

public class CommentViewModel
{
    public required string UserId { get; set; }
    public required string ArticleId { get; set; }
    public string? Content { get; set; }
}