using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Extentions;
using BiteWise.ViewModels.TagViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class TagController(IService<Tag> tagService) : Controller
{
    private readonly IService<Tag> _tagService = tagService;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTag(TagViewModel tagViewModel)
    {
        await _tagService.CreateAsync(new Tag()
        {
            UserId = tagViewModel.UserId,
            ArticleId = tagViewModel.ArticleId,
            Link = tagViewModel.Link
        });

        return View();
    }

    [Authorize]
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllTags(string articleId)
    {
        var model = await _tagService.GetAllAsync();

        return View(model.Where(t => t.ArticleId == articleId));
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