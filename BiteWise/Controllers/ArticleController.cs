using BiteWise.BLL.Models;
using BiteWise.BLL.Services;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.TablesСonnections;
using BiteWise.Extentions;
using BiteWise.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiteWise.Controllers;

public class ArticleController(IService<Article> articleService, IService<Tag> tagService, IService<TagArticleConnection> tagArticleConnectionService) : Controller
{
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
            SelectedTagsIds = new List<string>()
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
                await _tagArticleConnectionService.CreateAsyncTagArticleConnection(article.SelectedTagsIds, article);
            }

            return RedirectToAction("Mypage", "User");
        }
        return View("Article", articleViewModel);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditArticle(EditArticleViewModel editArticleViewModel)
    {
        if (ModelState.IsValid)
        {
            var article = await _articleService.GetAsync(editArticleViewModel.Id.ToString());

            if (article is not null) 
                await _articleService.UpdateAsync(article.Convert(editArticleViewModel));
        }

        return View();
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
            //var tagArticleConnections = _tagArticleConnectionService.GetAllAsync().Result.Where(c => c.ArticleEntityId == article.Id);
            //var tags = new List<Tag>();

            //foreach (var tagArticleConnection in tagArticleConnections)
            //{
            //    var tag = await _tagService.GetAsync(tagArticleConnection.TagEntityId.ToString());
            //    if (tag is not null)
            //        tags.Add(tag);
            //}

            var articalViewModel = new ArticleViewModel()
            {
                Image = article.Image,
                Title = article.Title,
                Content = article.Content,
                Created = article.Created,
                UserEntityId = article.UserEntityId,
                Tags = article.Tags
            };

            return View("PublicArticle", articalViewModel);
        }

        return View();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteArticle(string id)
    {
        await _articleService.DeleteAsync(await _articleService.GetAsync(id) ?? throw new ArgumentNullException());

        return View();
    }
}