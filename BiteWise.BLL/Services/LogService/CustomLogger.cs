using NLog;

namespace BiteWise.BLL.Services.LogService;

public class CustomLogger : ICustomLogger
{
    private readonly ILogger _logger;

    public CustomLogger()
    {
        LogManager.Setup().LoadConfiguration(builder =>
        {
            builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToConsole();
            builder.ForLogger().FilterMinLevel(LogLevel.Info).FilterMaxLevel(LogLevel.Info).WriteToFile(fileName: "${basedir}../../../Logs/UserLog/App_${shortdate}-userActions.txt");
            builder.ForLogger().FilterMinLevel(LogLevel.Debug).FilterMaxLevel(LogLevel.Fatal).WriteToFile(fileName: "${basedir}../../../Logs/DebugLog/App_${shortdate}-debug.txt");
        });

        var logger = LogManager.GetCurrentClassLogger();

        _logger = logger;
    }

    public void LoggingCritical(Exception ex)
    {
        var stack = string.Join(" ", ex.StackTrace?.Trim().Split(" ").Take(2) ?? ["No", "info"]);

        _logger.Fatal(string.Join(" | ", new { ex.Message, stack } ));
    }

    public void LoggingInfo(InfoTypes infoTypes, string username)
    {
        switch (infoTypes)
        {
            case InfoTypes.RegisterSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно зарегистриловался");
                break;
            case InfoTypes.UserEditSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно отредактировал профиль");
                break;
            case InfoTypes.UserRoleEditSuccseed:
                _logger.Info($"Добавлена новая роль пользователю под логином - {username}");
                break;
            case InfoTypes.UserRoleDeleteSuccseed:
                _logger.Info($"Удалена роль у пользователя под логином - {username}");
                break;
            case InfoTypes.LoginCompleted:
                _logger.Info($"Пользователь под логином - {username}, успешно зашел в приложение");
                break;
            case InfoTypes.LogOut:
                _logger.Info($"Пользователь под логином - {username}, успешно вышел из приложения");
                break;
            case InfoTypes.ArticleCreateSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно создал статью");
                break;
            case InfoTypes.TagCreateSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно создал тэг");
                break;
            case InfoTypes.CommentCreateSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно создал коментарий");
                break;
            case InfoTypes.ArticleEditSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно изменил статью");
                break;
            case InfoTypes.TagEditSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно изменил тег");
                break;
            case InfoTypes.CommentEditSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно изменил коментарий");
                break;
            case InfoTypes.ArticleDeleteSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно удалил статью");
                break;
            case InfoTypes.TagDeleteSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно удалил тэг");
                break;
            case InfoTypes.CommentDeleteSuccseed:
                _logger.Info($"Пользователь под логином - {username}, успешно удалил коментарий");
                break;
            default:
                break;
        }
    }

    public void LoggingUserError(UserErrorsType userErrorsType)
    {
        switch (userErrorsType)
        {
            case UserErrorsType.WrongLoginOrPassword:
                _logger.Error("Пользователь ввел неправильный логин или пароль");
                break;
            case UserErrorsType.RegisterError:
                _logger.Error("Пользователь ввел неправильные значения логин или пароль");
                break;
            case UserErrorsType.LoginExists:
                _logger.Error("Пользователь ввел существующий логин при регистрации");
                break;
            case UserErrorsType.UserRoleEditFailure:
                _logger.Error("Неудачное присвоение роли пользователю");
                break;
            case UserErrorsType.UserRoleDeleteFailur:
                _logger.Error("Неудачное удаление роли пользователю");
                break;
            case UserErrorsType.General:
                _logger.Error("Пользователь ввел некорректные данные");
                break;
            case UserErrorsType.Restricted_Area:
                break;
            default:
                break;
        }
    }
}

public enum UserErrorsType
{
    WrongLoginOrPassword,
    RegisterError,
    LoginExists,
    General,
    UserRoleEditFailure,
    UserRoleDeleteFailur,
    Restricted_Area
}

public enum InfoTypes
{
    RegisterSuccseed,
    LoginCompleted,
    LogOut,
    UserEditSuccseed,
    UserRoleEditSuccseed,
    UserRoleDeleteSuccseed,
    ArticleCreateSuccseed,
    TagCreateSuccseed,
    CommentCreateSuccseed,
    ArticleEditSuccseed,
    TagEditSuccseed,
    CommentEditSuccseed,
    ArticleDeleteSuccseed,
    TagDeleteSuccseed,
    CommentDeleteSuccseed
}