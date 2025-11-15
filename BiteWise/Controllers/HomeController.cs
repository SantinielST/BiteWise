using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Models;
using BiteWise.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BiteWise.Controllers;

public class HomeController(IService<Article> articleService) : Controller
{
    private readonly IService<Article> _articleService= articleService;

    public async Task<IActionResult> Index()
    {
        var model = new MainViewModel();
        model.DashBoardView.Articles = [.. _articleService.GetAllAsync().Result.OrderByDescending(a => a.Created)];

        return View(model);
    }

    public async Task<IActionResult> Register()
    {
        return View("Register");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AccessDeniedError()
    {
        return View("AccessDeniedError");
    }

    public IActionResult SomethingWentWrongError()
    {
        return View("SomethingWentWrongError");
    }
}