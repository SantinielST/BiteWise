using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.Extentions;
using BiteWise.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class CommentController(IService<Comment> commentService) : Controller
{
    private readonly IService<Comment> _commentService = commentService;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment(CommentViewModel commentViewModel)
    {
        await _commentService.CreateAsync(new Comment()
        {
            UserId = commentViewModel.UserId,
            ArticleId = commentViewModel.ArticleId,
            Content = commentViewModel.Content
        });

        return View();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditComment(EditCommentViewModel editCommentViewModel)
    {
        if (ModelState.IsValid)
        {
            var comment = await _commentService.GetAsync(editCommentViewModel.Id.ToString());

            if (comment is not null)
                await _commentService.UpdateAsync(comment.Convert(editCommentViewModel));
        }

        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllComments(string userId)
    {
        var model = await _commentService.GetAllAsync();

        return View(model.Where(c => c.UserId == userId));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetComment(string id)
    {
        return View(await _commentService.GetAsync(id));
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteComment(string id)
    {
        await _commentService.DeleteAsync(await _commentService.GetAsync(id)?? throw new ArgumentNullException());

        return View();
    }
}