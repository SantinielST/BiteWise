namespace BiteWise.ViewModels.CommentViewModels;

public class EditCommentViewModel
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public string? Content { get; set; }
    public required Guid ArticleId { get; set; }
}