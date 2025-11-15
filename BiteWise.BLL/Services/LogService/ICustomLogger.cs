namespace BiteWise.BLL.Services.LogService;

public interface ICustomLogger
{
    void LoggingInfo(InfoTypes infoTypes, string userName);
    void LoggingUserError(UserErrorsType userErrorsType);
    void LoggingCritical(Exception exception);
}
