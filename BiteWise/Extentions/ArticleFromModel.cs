using BiteWise.BLL.Models;
using BiteWise.ViewModels.ArticleViewModels;

namespace BiteWise.Extentions;

public static class ArticleFromModel
{
    public static Article Convert(this Article article, EditArticleViewModel editArticleViewModel)
    {
        article.Title = editArticleViewModel.Title;
        article.Content = editArticleViewModel.Content;
        article.UserEntityId = editArticleViewModel.UserEntityId;

        return article;
    }
}
