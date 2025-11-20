using BiteWise.BLL.Models;
using BiteWise.Contracts.ArticleDtos;

namespace BiteWise.Extentions;

public static class ArticleFromModel
{
    public static Article Convert(this Article article, EditArticleDto editArticleViewModel)
    {
        article.Title = editArticleViewModel.Title;
        article.Content = editArticleViewModel.Content;
        article.Image = editArticleViewModel.Image;
        article.SelectedTagsIds = editArticleViewModel.SelectedTagsIds?.ToList();

        return article;
    }
}