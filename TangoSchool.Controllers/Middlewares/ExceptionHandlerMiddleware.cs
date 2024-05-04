using System.Net;
using TangoSchool.ApplicationServices.Models.Common;

namespace TangoSchool.Middlewares;

internal class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware
    (
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger
    )
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
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            var response = exception switch
            {
                ApplicationException _ => new((int) HttpStatusCode.BadRequest, exception.Message),
                KeyNotFoundException _ => new((int) HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new((int) HttpStatusCode.Unauthorized, "Unauthorized."),
                _ => new ExceptionResponse((int) HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
