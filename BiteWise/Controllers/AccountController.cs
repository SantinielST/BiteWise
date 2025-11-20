using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

/// <summary>
/// Контроллер для работы с аккаунтом
/// </summary>
/// <param name="userService"></param>
/// <param name="customLogger"></param>
public class AccountController(IService<User> userService, ICustomLogger customLogger) : Controller
{
    private readonly ICustomLogger _customLogger = customLogger;
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
                    _customLogger.LoggingInfo(InfoTypes.LoginCompleted, model.Email);
                    await _userService.SignInAsync(model.Email, false);
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            _customLogger.LoggingUserError(UserErrorsType.WrongLoginOrPassword);
            ModelState.AddModelError(nameof(model.Email), " ");
            ModelState.AddModelError(nameof(model.Password), "Неправильный логин и (или) пароль");
        }
        _customLogger.LoggingUserError(UserErrorsType.WrongLoginOrPassword);
        return View(model);
    }

    [Route("Logout")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _userService.SignOutAsync();
        _customLogger.LoggingInfo(InfoTypes.LogOut, User.Identity?.Name ?? "Ошибка логина пользователя");
        return RedirectToAction("Index", "Home");
    }
}