using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Extentions;
using BiteWise.ViewModels;
using BiteWise.ViewModels.TagViewModels;
using Microsoft.AspNetCore.Authorization;
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
           
            return RedirectToAction("Create", "Article");
        }
        else
        {
            return View("CreateTag");
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditTag(EditTagViewModel editTagViewModel)
    {
        if (ModelState.IsValid)
        {
            var tag = await _tagService.GetAsync(editTagViewModel.Id.ToString());

            if (tag is not null)
            {
                await _tagService.UpdateAsync(tag.Convert(editTagViewModel));
            }
        }

        return View();
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTag(string id)
    {
        return View(await _tagService.GetAsync(id));
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteTag(string id)
    {
        await _tagService.DeleteAsync(await _tagService.GetAsync(id) ?? throw new ArgumentNullException());

        return View();
    }
}