using BiteWise.BLL.Models;
using BiteWise.Contracts.TagDtos;

namespace BiteWise.Extentions;

public static class TagFromModel
{
    public static Tag Convert(this Tag tag, EditTagDto editTagViewModel)
    {
        tag.Name = editTagViewModel.Name;

        return tag;
    }
}