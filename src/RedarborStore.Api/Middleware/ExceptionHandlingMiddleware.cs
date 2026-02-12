using System.Net;
using System.Text.Json;
using RedarborStore.Api.Models;
using RedarborStore.Domain.Exceptions;

namespace RedarborStore.Api.Middleware;

public class ExceptionHandlingMiddleware
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = context.TraceIdentifier;
        var (statusCode, message, errors) = exception switch
        {
            NotFoundException notFound => (
                HttpStatusCode.NotFound,
                notFound.Message,
                (IDictionary<string, string[]>?)null
            ),
            ValidationException validation => (
                HttpStatusCode.BadRequest,
                validation.Message,
                validation.Errors
            ),
            BusinessRuleException businessRule => (
                HttpStatusCode.UnprocessableEntity,
                businessRule.Message,
                (IDictionary<string, string[]>?)null
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.",
                (IDictionary<string, string[]>?)null
            )
        };
        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception | TraceId: {TraceId}", traceId);
        else
            _logger.LogWarning("Handled exception: {Message} | TraceId: {TraceId}", exception.Message, traceId);

        var response = new ApiErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            TraceId = traceId,
            Errors = errors
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}