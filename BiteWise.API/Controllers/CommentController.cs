using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.Contracts.CommentDto;
using BiteWise.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController(IService<Comment> commentService, ICustomLogger customLogger) : ControllerBase
{
    private readonly ICustomLogger _customLogger = customLogger;
    private readonly IService<Comment> _commentService = commentService;

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto commentViewModel)
    {
        if (ModelState.IsValid)
        {
            var id = Guid.NewGuid();
            await _commentService.CreateAsync(new Comment()
            {
                Id = id,
                UserEntityId = commentViewModel.UserId,
                ArticleId = commentViewModel.ArticleId,
                Content = commentViewModel.Content,
                Created = DateTime.Now
            });

            _customLogger.LoggingInfo(InfoTypes.CommentCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return StatusCode(201, $"Новый коментарий создан. Идентификатор: {id}");
        }
        else
        {
            _customLogger.LoggingUserError(UserErrorsType.General);
            return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditComment([FromBody] EditCommentDto editCommentViewModel)
    {
        if (ModelState.IsValid)
        {
            var comment = await _commentService.GetAsync(editCommentViewModel.Id.ToString());

            if (comment is not null)
            {
                await _commentService.UpdateAsync(comment.Convert(editCommentViewModel));
                _customLogger.LoggingInfo(InfoTypes.CommentCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
                return StatusCode(201, $"Коментарий изменён. Идентификатор: {comment.Id}");
            }
            else
            {
                _customLogger.LoggingUserError(UserErrorsType.General);
                return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
            }
        }

        _customLogger.LoggingUserError(UserErrorsType.General);
        return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var request = await _commentService.GetAllAsync();

        return StatusCode(200, request);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteComment([FromRoute] string commentId)
    {
        var comment = await _commentService.GetAsync(commentId.ToString());

        if (comment is not null)
        {
            await _commentService.DeleteAsync(comment);
            _customLogger.LoggingInfo(InfoTypes.CommentDeleteSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return StatusCode(201, $"Коментарий удален. Идентификатор: {comment.Id}");
        }
        return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
    }
}