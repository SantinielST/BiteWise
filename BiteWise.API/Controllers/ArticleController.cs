using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.Contracts.ArticleDtos;
using BiteWise.DLL.TablesСonnections;
using BiteWise.Extentions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiteWise.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController(IService<Article> articleService,
    IService<Tag> tagService,
    IService<TagArticleConnection> tagArticleConnectionService,
    ICustomLogger customLogger) : ControllerBase
{
    private readonly ICustomLogger _customLogger = customLogger;
    private readonly IService<Article> _articleService = articleService;
    private readonly IService<Tag> _tagService = tagService;
    private readonly IService<TagArticleConnection> _tagArticleConnectionService = tagArticleConnectionService;

    [HttpPost]
    public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDto articleViewModel)
    {
        if (ModelState.IsValid)
        {
            var article = new Article()
            {
                Id = Guid.NewGuid(),
                Title = articleViewModel.Title,
                Content = articleViewModel.Content,
                Image = articleViewModel.Image,
                Created = DateTime.Now,
                UserEntityId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                SelectedTagsIds = articleViewModel.SelectedTagsIds?.ToList()
            };

            if (article.SelectedTagsIds is not null)
            {
                await _articleService.CreateAsync(article);
                await _tagArticleConnectionService.CreateAsyncTagArticleConnections(article.SelectedTagsIds, article);
            }

            _customLogger.LoggingInfo(InfoTypes.ArticleCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return StatusCode(201, $"Новая статья {article.Title} создана. Идентификатор: {article.Id}");
        }

        _customLogger.LoggingUserError(UserErrorsType.General);
        return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
    }

    [HttpPut]
    public async Task<IActionResult> EditArticle([FromBody] EditArticleDto editArticleViewModel)
    {
        if (ModelState.IsValid)
        {
            var article = await _articleService.GetAsync(editArticleViewModel.Id.ToString());
            var connections = _tagArticleConnectionService.GetAllAsync().Result;

            if (article is not null)
            {
                await _articleService.UpdateAsync(article.Convert(editArticleViewModel));

                if (article.SelectedTagsIds is not null)
                {
                    foreach (var connection in connections)
                    {
                        for (int i = 0; i < article.SelectedTagsIds.Count; i++)
                        {
                            if (connection.ArticleEntityId == article.Id && connection.TagEntityId.ToString() == article.SelectedTagsIds[i])
                            {
                                if (article.Tags is not null)
                                    article.Tags.Remove(article.Tags.First(t => t.Id.ToString() == article.SelectedTagsIds[i]));

                                article.SelectedTagsIds.Remove(article.SelectedTagsIds[i]);
                            }
                        }
                    }

                    await _tagArticleConnectionService.CreateAsyncTagArticleConnections(article.SelectedTagsIds, article);
                }

                if (article.Tags is not null)
                {
                    foreach (var tag in article.Tags)
                    {
                        if (article.SelectedTagsIds is null || article.SelectedTagsIds.Count == 0 || !article.SelectedTagsIds.Contains(tag.Id.ToString()))
                        {
                            await _tagArticleConnectionService.DeleteAsync(connections.Where(c => c.TagEntityId == tag.Id && c.ArticleEntityId == article.Id).FirstOrDefault() ?? throw new NullReferenceException());
                        }
                    }
                }
                _customLogger.LoggingInfo(InfoTypes.ArticleEditSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
                return StatusCode(201, $"Cтатья {article.Title} изменена. Идентификатор: {article.Id}");
            }
        }

        return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllArticles()
    {
        var request = await _articleService.GetAllAsync();

        return StatusCode(200, request);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteArticle([FromRoute] string id)
    {
        var article = await _articleService.GetAsync(id);
        await _articleService.DeleteAsync(article ?? throw new ArgumentNullException());
        _customLogger.LoggingInfo(InfoTypes.ArticleDeleteSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
        return StatusCode(201, $"Cтатья {article.Title} удалена. Идентификатор: {article.Id}");
    }
}