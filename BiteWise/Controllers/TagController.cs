using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.Extentions;
using BiteWise.ViewModels;
using BiteWise.ViewModels.TagViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class TagController(IService<Tag> tagService, ICustomLogger customLogger) : Controller
{
    private readonly ICustomLogger _customLogger = customLogger;
    private readonly IService<Tag> _tagService = tagService;

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View("CreateTag");
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(TagViewModel tagViewModel)
    {
        if (ModelState.IsValid)
        {
            await _tagService.CreateAsync(new Tag()
            {
                Name = tagViewModel.Name
            });

            _customLogger.LoggingInfo(InfoTypes.TagCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return RedirectToAction("GetAllTags", "Tag");
        }
        else
        {
            _customLogger.LoggingUserError(UserErrorsType.General);
            return View("CreateTag");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditTag(EditTagViewModel editTagViewModel)
    {
        if (ModelState.IsValid)
        {
            var tag = await _tagService.GetAsync(editTagViewModel.Id.ToString());

            if (tag is not null)
            {
                await _tagService.UpdateAsync(tag.Convert(editTagViewModel));
            }
            _customLogger.LoggingInfo(InfoTypes.TagEditSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return RedirectToAction("GetAllTags", "Tag");
        }
        _customLogger.LoggingUserError(UserErrorsType.General);
        return View("EditTag");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags()
    {
        var model = new AllTagsViewModel()
        {
            Tags = [.. _tagService.GetAllAsync().Result]
        };

        return View("Tags", model);
    }

    [HttpGet]
    public async Task<IActionResult> GetTag(string id)
    {
        return View("EditTag", await _tagService.GetAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetTagForEdit(string id)
    {
        var tag = await _tagService.GetAsync(id);

        if (tag is not null)
        {
            var model = new EditTagViewModel()
            {
                Id = tag.Id,
                Name = tag.Name
            };
            return View("EditTag", model);
        }
        return View("Tags");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTag(string id)
    {
        await _tagService.DeleteAsync(await _tagService.GetAsync(id) ?? throw new ArgumentNullException());
        _customLogger.LoggingInfo(InfoTypes.TagDeleteSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
        return RedirectToAction("GetAllTags", "Tag");
    }
}