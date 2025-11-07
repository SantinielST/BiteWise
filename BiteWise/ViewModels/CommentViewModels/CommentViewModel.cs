namespace BiteWise.ViewModels.CommentViewModels;

public class CommentViewModel
{
    public Guid UserId { get; set; }
    public  Guid ArticleId { get; set; }
    public string? Content { get; set; }
}