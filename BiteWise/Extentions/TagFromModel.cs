using BiteWise.BLL.Models;
using BiteWise.ViewModels.TagViewModels;

namespace BiteWise.Extentions;

/// <summary>
/// Расширение для ручного маппинга тегов
/// </summary>
public static class TagFromModel
{
    public static Tag Convert(this Tag tag, EditTagViewModel editTagViewModel)
    {
        tag.Name = editTagViewModel.Name;

        return tag;
    }
}