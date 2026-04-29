using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);  // kör resten av pipeline
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Response has already started, cannot modify headers.");
            return;
        }
        var (statusCode, message) = exception switch
        {
            KeyNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            InvalidOperationException ex => (StatusCodes.Status400BadRequest, ex.Message),
            UnauthorizedAccessException ex => (StatusCodes.Status403Forbidden, ex.Message),
            ArgumentException ex => (StatusCodes.Status400BadRequest, ex.Message),
            DbUpdateException ex when ex.InnerException?.Message.Contains("unique") == true
                                            => (StatusCodes.Status409Conflict, "En post med samma värde finns redan."),
            _ => (StatusCodes.Status500InternalServerError, "Något gick fel på servern.")
        };

        // Logga alltid — 500:or som error, övriga som warning
        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Ohanterat fel: {Message}", exception.Message);
        else
            _logger.LogWarning("Hanterat fel {StatusCode}: {Message}", statusCode, message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            error = message,
            statusCode
        });
    }
}