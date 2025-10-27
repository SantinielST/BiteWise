using BiteWise.BLL.Models;
using BiteWise.ViewModels.CommentViewModels;

namespace BiteWise.Extentions;

public static class CommentFromModel
{
    public static Comment Convert(this Comment comment, EditCommentViewModel editCommentViewModel)
    {
        comment.Content = editCommentViewModel.Content;
        comment.ArticleId = editCommentViewModel.ArticleId;
        comment.Id = editCommentViewModel.Id;

        return comment;
    }
}