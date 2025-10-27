using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Extentions;
using BiteWise.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiteWise.Controllers;

public class ArticleController(IService<Article> articleService) : Controller
{
    private readonly IService<Article> _articleService = articleService;

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View("Article");
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle(ArticleViewModel articleViewModel)
    {
        if (ModelState.IsValid)
        {
            await _articleService.CreateAsync(new Article()
            {
                Id = new Guid(),
                Title = articleViewModel.Title,
                Content = articleViewModel.Content,
                Image = articleViewModel.Image,
                Created = DateTime.Now,
                UserEntityId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });

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
            var articalViewModel = new ArticleViewModel()
            {
                Image = article.Image,
                Title = article.Title,
                Content = article.Content,
                Created = article.Created,
                UserEntityId = article.UserEntityId
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