using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.BLL.Services.LogService;
using BiteWise.Contracts;
using BiteWise.Contracts.UserDtos;
using BiteWise.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace BiteWise.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IService<User> userService, ICustomLogger customLogger) : ControllerBase
    {
        private readonly ICustomLogger _customLogger = customLogger;
        private readonly IService<User> _userService = userService;

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registeViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetAsync(registeViewModel.EmailReg ?? throw new ArgumentNullException());

                if (user != null)
                {
                    _customLogger.LoggingUserError(UserErrorsType.LoginExists);
                    return StatusCode(400, $"Ошибка: Пользователь с email {registeViewModel.UserName} уже зарегистрирован. Выберите другой email!");
                }

                var newUser = new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = registeViewModel.EmailReg ?? throw new ArgumentNullException(),
                    Password = registeViewModel.PasswordReg,
                    UserName = registeViewModel.EmailReg
                };

                var result = await _userService.CreateUserAsync(newUser);

                if (result.Succeeded)
                {
                    _customLogger.LoggingInfo(InfoTypes.RegisterSuccseed, registeViewModel.EmailReg);
                    _customLogger.LoggingInfo(InfoTypes.LoginCompleted, registeViewModel.EmailReg);
                    await _userService.SignInAsync(registeViewModel.EmailReg, false);

                    return StatusCode(201, $"Пользователь {newUser.UserName} добавлен. Идентификатор: {newUser.Id}");
                }
                else
                {
                    return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
                }
            }
            else
            {
                return StatusCode(400, $"Ошибка: Произошла ошибка с валидацией данных!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserList() // реализовать поиск!
        {
            var users = await _userService.GetAllAsync();

            var request = new SearchViewDto()
            {
                UserList = [.. users]
            };
            return StatusCode(200, request);
        }

        [HttpPut]
        public async Task<IActionResult> EditUser(
        [FromRoute] Guid id,
        [FromBody] UserEditDto editViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetByIdAsync(id.ToString() ?? string.Empty);

                if (user is not null)
                {
                    _customLogger.LoggingInfo(InfoTypes.UserEditSuccseed, user.UserName);
                    await _userService.UpdateAsync(user.Convert(editViewModel));
                    return StatusCode(201, $"Пользователь {user.UserName} обновлен. Идентификатор: {user.Id}");
                }
                return StatusCode(400, $"Ошибка: Произошла ошибка с пользовательскими данными!");
            }
            _customLogger.LoggingUserError(UserErrorsType.General);
            return StatusCode(400, $"Ошибка: Произошла ошибка с пользовательскими данными!");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(
             [FromRoute] Guid id
            )
        {
            var user = await _userService.GetByIdAsync(id.ToString());
            return StatusCode(201, $"Пользователь {user.UserName} удален. Идентификатор: {id}");
        }
    }
}