using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.Extentions;
using BiteWise.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

public class CommentController(IService<Comment> commentService, ICustomLogger customLogger) : Controller
{
    private readonly ICustomLogger _customLogger = customLogger;
    private readonly IService<Comment> _commentService = commentService;

    [HttpPost]
    public async Task<IActionResult> CreateComment(CommentViewModel commentViewModel)
    {
        if (ModelState.IsValid)
        {
            await _commentService.CreateAsync(new Comment()
            {
                UserEntityId = commentViewModel.UserId,
                ArticleId = commentViewModel.ArticleId,
                Content = commentViewModel.Content,
                Created = DateTime.Now
            });

            _customLogger.LoggingInfo(InfoTypes.CommentCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
        }
        else
        {
            _customLogger.LoggingUserError(UserErrorsType.General);
        }

        return RedirectToAction("GetArticle", "Article", new { id = commentViewModel.ArticleId.ToString() });
    }

    [HttpPost]
    public async Task<IActionResult> EditComment(EditCommentViewModel editCommentViewModel)
    {
        if (ModelState.IsValid)
        {
            var comment = await _commentService.GetAsync(editCommentViewModel.Id.ToString());

            if (comment is not null)
            {
                await _commentService.UpdateAsync(comment.Convert(editCommentViewModel));
                _customLogger.LoggingInfo(InfoTypes.CommentCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            }
            else
            {
                _customLogger.LoggingUserError(UserErrorsType.General);
            }

            return RedirectToAction("GetArticle", "Article", new { id = editCommentViewModel?.ArticleId.ToString() });
        }

        _customLogger.LoggingUserError(UserErrorsType.General);
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

        if (comment is not null)
        {
            var model = new EditCommentViewModel()
            {
                Content = comment.Content,
                Id = comment.Id,
                UserId = comment.UserEntityId,
                ArticleId = comment.ArticleId
            };

            return View("EditComment", model);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var comment = await _commentService.GetAsync(commentId.ToString());

        if (comment is not null)
        {
            await _commentService.DeleteAsync(comment);
            _customLogger.LoggingInfo(InfoTypes.CommentDeleteSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return RedirectToAction("GetArticle", "Article", new { id = comment?.ArticleId.ToString() });
        }
        return RedirectToAction("GetArticle", "Article", new { id = comment?.ArticleId.ToString() });
    }
}