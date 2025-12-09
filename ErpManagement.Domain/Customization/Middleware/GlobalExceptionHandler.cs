
namespace ErpManagement.Domain.Customization.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        string text = new SelectListForExcLog()
        {
            Message = exception.Message,
            Error = exception.Message + (exception.InnerException == null ? string.Empty : exception.InnerException.Message)
        }.ToString() ?? string.Empty;

        await httpContext.Response.WriteAsync(text, cancellationToken);
        return true;
    }
}
