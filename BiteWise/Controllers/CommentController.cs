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

    [HttpPost]
    public async Task<IActionResult> CreateComment(CommentViewModel commentViewModel)
    {
        await _commentService.CreateAsync(new Comment()
        {
            UserEntityId = commentViewModel.UserId,
            ArticleId = commentViewModel.ArticleId,
            Content = commentViewModel.Content,
            Created = DateTime.Now
        });

        return RedirectToAction("GetArticle", "Article", new { id = commentViewModel.ArticleId.ToString() });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(EditCommentViewModel editCommentViewModel)
    {
        if (ModelState.IsValid)
        {
            var comment = await _commentService.GetAsync(editCommentViewModel.Id.ToString());

            if (comment is not null)
                await _commentService.UpdateAsync(comment.Convert(editCommentViewModel));

            return RedirectToAction("GetArticle", "Article", new { id = editCommentViewModel?.ArticleId.ToString() });
        }
        return RedirectToAction("GetArticle", "Article", new { id = editCommentViewModel?.ArticleId.ToString() });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllComments(string userId)
    {
        var model = await _commentService.GetAllAsync();

        return View(model.Where(c => c.UserEntityId.ToString() == userId));
    }

    [HttpGet]
    public async Task<IActionResult> GetComment(string commentId)
    {
        var comment = await _commentService.GetAsync(commentId);

        var model = new EditCommentViewModel()
        {
            Content = comment.Content,
            Id = comment.Id,
            UserId = comment.UserEntityId,
            ArticleId = comment.ArticleId
        };
        return View("EditComment", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var comment = await _commentService.GetAsync(commentId.ToString());
        await _commentService.DeleteAsync(comment);

        return RedirectToAction("GetArticle", "Article", new { id = comment?.ArticleId.ToString() });
    }
}