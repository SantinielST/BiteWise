namespace BiteWise.ViewModels.TagViewModels;

public class EditTagViewModel
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string ArticleId { get; set; }
    public string? Link { get; set; }
}