using BiteWise.BLL.Models;
using BiteWise.ViewModels.TagViewModels;

namespace BiteWise.Extentions;

public static class TagFromModel
{
    public static Tag Convert(this Tag tag, EditTagViewModel editTagViewModel)
    {
        tag.UserId = editTagViewModel.UserId;
        tag.ArticleId = editTagViewModel.ArticleId;
        tag.Link = editTagViewModel.Link;

        return tag;
    }
}