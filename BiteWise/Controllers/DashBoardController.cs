using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class DashBoardController(IService<Article> articleService) : Controller
{
    private readonly IService<Article> _articleService = articleService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var articles = await _articleService.GetAllAsync();

        var dashboardViewModel = new DashBoardViewModel()
        {
            Articles = [.. articles.OrderByDescending(a => a.Created)],
        };

        return View(dashboardViewModel);
    }
}