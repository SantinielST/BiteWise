using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Extentions;
using BiteWise.ViewModels;
using BiteWise.ViewModels.TagViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class TagController(IService<Tag> tagService) : Controller
{
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

            return RedirectToAction("GetAllTags", "Tag");
        }
        else
        {
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
            return RedirectToAction("GetAllTags", "Tag");
        }

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

        return RedirectToAction("GetAllTags", "Tag");
    }
}