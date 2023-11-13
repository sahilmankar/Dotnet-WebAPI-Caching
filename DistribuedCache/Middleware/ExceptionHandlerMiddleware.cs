using DistributedCache.Exceptions;
using System.Text.Json;

namespace DistributedCache.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserNotFoundException ex)
        {
            _logger.LogInformation($"User not found: {ex.Message}");

            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = "Internal Server Error" });
            await context.Response.WriteAsync(result);
        }
    }
}
