using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Extentions;
using BiteWise.ViewModels;
using BiteWise.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BiteWise.Controllers;

public class UserController(IService<User> userService, IService<Article> _articleService) : Controller
{
    private readonly IService<User> _userService = userService;
    private readonly IService<Article> _articleService = _articleService;

    [Route("Mypage")]
    [HttpGet]
    public async Task<IActionResult> MyPage()
    {
        var user = await _userService.GetByUserAsync(User);
        var model = new UserViewModel(user);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registeViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetAsync(registeViewModel.EmailReg ?? throw new ArgumentNullException());

            if (user != null)
            {
                TempData["Error"] = "Данный email адрес уже был зарегистрирован";
                return View(registeViewModel);
            }

            var result = await _userService.CreateUserAsync(new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = registeViewModel.EmailReg ?? throw new ArgumentNullException(),
                Password = registeViewModel.PasswordReg,
                UserName = registeViewModel.EmailReg
            });

            if (result.Succeeded)
            {
                await _userService.SignInAsync(registeViewModel.EmailReg, false);
                return RedirectToAction("MyPage");
            }
            else
            {
                return View(registeViewModel);
            }
        }
        else
        {
            return View(registeViewModel);
        }
    }

    [Route("UserList")]
    [HttpGet]
    public async Task<IActionResult> UserList(string search) // реализовать поиск!
    {
        var users = await _userService.GetAllAsync();

        var model = new SearchViewModel()
        {
            UserList = [.. users]
        };
        return View("UserList", model);
    }

    [Route("EditUser")]
    [HttpGet]
    public async Task<IActionResult> EditUser()
    {
        var user = await _userService.GetByUserAsync(User);
        var model = new UserEditViewModel()
        {
            UserId = user.Id ?? string.Empty,
            Email = user.Email,
            Image = user.Image,
            About = user.About,
            Status = user.Status,
            Roles = user.Roles,
            ReturnUrl = null
        };

        return View(model);
    }

    [Route("UpdateUser")]
    [HttpPost]
    public async Task<IActionResult> UpdateUser(UserEditViewModel editViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

            if (user is not null)
            {
                await _userService.UpdateAsync(user.Convert(editViewModel));
                return RedirectToAction("MyPage");
            }
        }

        return RedirectToAction("EditUser");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserRole(SearchViewModel searchViewModel)
    {
        if (ModelState.IsValid)
        {
            ArgumentNullException.ThrowIfNull(searchViewModel.UserId);
            var user = await _userService.GetByIdAsync(searchViewModel.UserId);

            if (user is not null)
            {
                ArgumentNullException.ThrowIfNull(searchViewModel.RoleName);
                await _userService.UpdateRolesAsync(user, searchViewModel.RoleName);

                return RedirectToAction("UserList");
            }
        }

        return RedirectToAction("UserList");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUserRole(string role, string userId)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetByIdAsync(userId);

            if (user is not null)
            {
                await _userService.UpdateRolesAsync(user, role);
                return RedirectToAction("UserList");
            }
        }

        return RedirectToAction("UserList");
    }

    [HttpGet]
    public async Task<IActionResult> DeleteUser()
    {
        return View("DeleteUser");
    }
}