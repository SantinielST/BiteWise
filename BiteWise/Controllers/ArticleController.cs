using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.DLL.TablesСonnections;
using BiteWise.Extentions;
using BiteWise.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiteWise.Controllers;

public class ArticleController(IService<Article> articleService,
    IService<Tag> tagService,
    IService<TagArticleConnection> tagArticleConnectionService,
    ICustomLogger customLogger) : Controller
{
    private readonly ICustomLogger _customLogger = customLogger;
    private readonly IService<Article> _articleService = articleService;
    private readonly IService<Tag> _tagService = tagService;
    private readonly IService<TagArticleConnection> _tagArticleConnectionService = tagArticleConnectionService;

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateArticleViewModel()
        {
            Created = DateTime.Now,
            AllTags = _tagService.GetAllAsync().Result.ToList(),
            SelectedTagsIds = []
        };
        return View("Article", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle(CreateArticleViewModel articleViewModel)
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
            return RedirectToAction("Mypage", "User");
        }

        _customLogger.LoggingUserError(UserErrorsType.General);
        return View("Article", articleViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> EditArticle(Guid id)
    {
        var article = await _articleService.GetAsync(id.ToString());

        if (article != null)
        {
            var model = new EditArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Image = article.Image,
                Tags = article.Tags,
                SelectedTagsIds = [],
                UserEntityId = article.UserEntityId,
                AllTags = _tagService.GetAllAsync().Result.ToList()
            };
            return View(model);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateArticle(EditArticleViewModel editArticleViewModel)
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
                        if (article.SelectedTagsIds is null|| article.SelectedTagsIds.Count == 0 || !article.SelectedTagsIds.Contains(tag.Id.ToString()))
                        {
                            await _tagArticleConnectionService.DeleteAsync(connections.Where(c => c.TagEntityId == tag.Id && c.ArticleEntityId == article.Id).FirstOrDefault() ?? throw new NullReferenceException());
                        }
                    }
                }
                _customLogger.LoggingInfo(InfoTypes.ArticleEditSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            }
        }

        return RedirectToAction("GetArticle", new { id = editArticleViewModel.Id.ToString() });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllArticles(string userId)
    {
        var model = await _articleService.GetAllAsync();

        return View(model.Where(a => a.UserEntityId == userId).OrderByDescending(a => a.Created).ToList());
    }

    [HttpGet]
    public async Task<IActionResult> GetArticle(string id)
    {
        var article = await _articleService.GetAsync(id);

        if (article is not null)
        {
            var articalViewModel = new ArticleViewModel()
            {
                Id = article.Id,
                Image = article.Image,
                Title = article.Title,
                Content = article.Content,
                Created = article.Created,
                UserEntityId = article.UserEntityId,
                Tags = article.Tags,
                Comments = article.Comments
            };

            return View("PublicArticle", articalViewModel);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteArticle(string id)
    {
        await _articleService.DeleteAsync(await _articleService.GetAsync(id) ?? throw new ArgumentNullException());
        _customLogger.LoggingInfo(InfoTypes.ArticleDeleteSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
        return RedirectToAction("Index", "DashBoard");
    }
}