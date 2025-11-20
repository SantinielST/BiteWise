using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IService<User> userService, ICustomLogger customLogger) : ControllerBase
    {
        private readonly ICustomLogger _customLogger = customLogger;
        private readonly IService<User> _userService = userService;

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(
            [FromRoute] Guid id,
            [FromForm] string roleName)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetByIdAsync(id.ToString());

                if (user is not null && roleName is not null)
                {
                    await _userService.UpdateRolesAsync(user, roleName);
                    _customLogger.LoggingInfo(InfoTypes.UserRoleEditSuccseed, user.UserName);
                    return StatusCode(201, $"Роль пользователя {user.UserName} обновлен. Идентификатор: {user.Id}");
                }
                return StatusCode(400, $"Ошибка: Произошла ошибка с пользовательскими данными!");
            }
            _customLogger.LoggingUserError(UserErrorsType.UserRoleEditFailure);
            return StatusCode(400, $"Ошибка: Произошла ошибка с пользовательскими данными!");
        }

        [HttpPatch]
        public async Task<IActionResult> DeleteUserRole(
            [FromRoute] Guid id,
            [FromBody] string role)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetByIdAsync(id.ToString());

                if (user is not null)
                {
                    if (await _userService.UpdateRolesAsync(user, role))
                    {
                        _customLogger.LoggingInfo(InfoTypes.UserRoleDeleteSuccseed, user.UserName);
                        return StatusCode(201, $"Роль пользователя {user.UserName} удалена. Идентификатор: {user.Id}");
                    }

                    else
                    {
                        _customLogger.LoggingUserError(UserErrorsType.UserRoleDeleteFailur);
                        return StatusCode(400, $"Ошибка: У пользователя не может быть меньше одной роли!");
                    }
                }
                else
                {
                    _customLogger.LoggingUserError(UserErrorsType.UserRoleDeleteFailur);
                    return StatusCode(400, $"Ошибка: Произошла ошибка с пользовательскими данными!");
                }
            }
            _customLogger.LoggingUserError(UserErrorsType.UserRoleDeleteFailur);
            return StatusCode(400, $"Ошибка: Произошла ошибка с пользовательскими данными!");
        }
    }
}