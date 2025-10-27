using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class AccountController(IService<User> userService) : Controller
{
    private readonly IService<User> _userService = userService;

    [HttpGet]
    [Route("Login")]
    public IActionResult Login(string returnUrl = "")
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [Route("Login")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Email is not null && model.Password is not null)
            {
                var result = await _userService.CheckPasswordAsync(model.Email, model.Password);

                if (result)
                {
                    await _userService.SignInAsync(model.Email, false);
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ModelState.AddModelError(nameof(model.Email), " ");
            ModelState.AddModelError(nameof(model.Password), "Неправильный логин и (или) пароль");

        }

        return View(model);
    }

    [Route("Logout")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _userService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}