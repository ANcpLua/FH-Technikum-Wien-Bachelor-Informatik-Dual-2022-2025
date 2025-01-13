using System.ComponentModel.DataAnnotations;
using Contract.Logger;

namespace PaperlessREST;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPaperlessLogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, IPaperlessLogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Proceed to the next middleware in the pipeline.
        }
        catch (Exception ex)
        {
            _logger.LogError("API", "UnhandledException", $"Error processing {context.Request.Path}", ex);

            // Handle the exception and return an appropriate response.
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json"; 

        var (statusCode, message) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = statusCode;

        // Write the error message to the response body.
        await context.Response.WriteAsJsonAsync(new { message });
    }
}
