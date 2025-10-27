namespace BiteWise.ViewModels.CommentViewModels;

public class EditCommentViewModel
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string ArticleId { get; set; }
    public string? Content { get; set; }
}