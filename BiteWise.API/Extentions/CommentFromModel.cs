using BiteWise.BLL.Models;
using BiteWise.Contracts.CommentDto;

namespace BiteWise.Extentions;

public static class CommentFromModel
{
    public static Comment Convert(this Comment comment, EditCommentDto editCommentViewModel)
    {
        comment.Content = editCommentViewModel.Content;
        comment.ArticleId = editCommentViewModel.ArticleId;
        comment.UserEntityId = editCommentViewModel.UserId;
        comment.Id = editCommentViewModel.Id;

        return comment;
    }
}