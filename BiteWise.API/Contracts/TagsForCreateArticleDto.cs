using BiteWise.BLL.Models;

namespace BiteWise.Contracts;

public class TagsForCreateArticleDto
{
    public IList<Tag>? Tags { get; set; }
}