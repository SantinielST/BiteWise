namespace BiteWise.DLL.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    public Guid UserEntityId { get; set; }
    public string? Content { get; set; }
}