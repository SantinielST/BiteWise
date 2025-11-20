using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.Contracts;
using BiteWise.Contracts.TagDtos;
using BiteWise.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.Controllers;

[ApiController]
[Route("[controller]")]
public class TagController(IService<Tag> tagService, ICustomLogger customLogger) : ControllerBase
{
    private readonly ICustomLogger _customLogger = customLogger;
    private readonly IService<Tag> _tagService = tagService;

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagDto tagViewModel)
    {
        if (ModelState.IsValid)
        {
            var id = Guid.NewGuid();

            await _tagService.CreateAsync(new Tag()
            {
                Id = id,
                Name = tagViewModel.Name
            });

            _customLogger.LoggingInfo(InfoTypes.TagCreateSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return StatusCode(201, $"Новый тег {tagViewModel.Name} создан. Идентификатор: {id}");
        }
        else
        {
            _customLogger.LoggingUserError(UserErrorsType.General);
            return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditTag([FromBody] EditTagDto editTagViewModel)
    {
        if (ModelState.IsValid)
        {
            var tag = await _tagService.GetAsync(editTagViewModel.Id.ToString());

            if (tag is not null)
            {
                await _tagService.UpdateAsync(tag.Convert(editTagViewModel));
            }
            _customLogger.LoggingInfo(InfoTypes.TagEditSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
            return StatusCode(201, $"Тег {editTagViewModel.Name} изменён. Идентификатор: {editTagViewModel.Id}"); ;
        }
        _customLogger.LoggingUserError(UserErrorsType.General);
        return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags()
    {
        var request = new AllTagsDto()
        {
            Tags = [.. _tagService.GetAllAsync().Result]
        };

        return StatusCode(200, request);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTag([FromRoute] string id)
    {
        var tag = await _tagService.GetAsync(id);
        await _tagService.DeleteAsync(tag ?? throw new ArgumentNullException());
        _customLogger.LoggingInfo(InfoTypes.TagDeleteSuccseed, User.Identity?.Name ?? "Ошибка логина пользователя");
        return StatusCode(201, $"Тег {tag.Name} удален. Идентификатор: {tag.Id}");
    }
}