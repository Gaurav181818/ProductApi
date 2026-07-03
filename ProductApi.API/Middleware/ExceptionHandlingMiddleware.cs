using System.Net;
using System.Text.Json;
using ProductApi.Domain.Exceptions;

namespace ProductApi.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        //catch (Exception ex)
        //{
        //    logger.LogError(ex, "Unhandled exception");
        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        //}
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                exception = ex.GetType().FullName,
                message = ex.Message,
                innerException = ex.InnerException?.Message,
                stackTrace = ex.StackTrace
            });
        }
    }
}