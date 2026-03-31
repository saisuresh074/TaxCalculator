using System.Net;
using System.Text.Json;
using TaxCalculator.Domain.Exceptions;

namespace TaxCalculator.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            CountryConfigNotFoundException e  => (HttpStatusCode.NotFound,           e.Message),
            InvalidTaxConfigurationException e => (HttpStatusCode.BadRequest,         e.Message),
            NoStrategyRegisteredException e    => (HttpStatusCode.BadRequest,         e.Message),
            ArgumentException e                => (HttpStatusCode.BadRequest,         e.Message),
            _                                  => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        var body = JsonSerializer.Serialize(new
        {
            error      = message,
            statusCode = (int)statusCode
        });

        return context.Response.WriteAsync(body);
    }
}
