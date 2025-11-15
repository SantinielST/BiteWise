using BiteWise.BLL.Services.LogService;

namespace BiteWise.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ICustomLogger customLogger)
{
    private readonly RequestDelegate _next = next;
    private readonly ICustomLogger _customLogger = customLogger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _customLogger.LoggingCritical(ex);

        context.Response.Redirect("/Home/SomethingWentWrongError");
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}