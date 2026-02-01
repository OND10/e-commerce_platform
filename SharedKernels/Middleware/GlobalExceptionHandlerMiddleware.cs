using SharedKernels.Results;
using SharedKernels.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernels.Middleware
{
    /// <summary>
    /// Global exception handler middleware with ProblemDetails support (RFC 7807)
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            // Get or create correlation ID
            var correlationId = context.TraceIdentifier;
            
            _logger.LogError(exception, 
                "An error occurred while processing request. CorrelationId: {CorrelationId}", 
                correlationId);

            var (statusCode, error) = exception switch
            {
                Exceptions.ValidationException validationEx => (
                    HttpStatusCode.BadRequest,
                    Error.Validation("Validation", "One or more validation errors occurred.", 
                        validationEx.Errors.Select(e => e.Description).ToArray())
                ),
                
                NotFoundException notFoundEx => (
                    HttpStatusCode.NotFound,
                    Error.NotFound(notFoundEx.EntityName, notFoundEx.Message)
                ),
                
                ConflictException conflictEx => (
                    HttpStatusCode.Conflict,
                    Error.Conflict("Conflict", conflictEx.Message)
                ),
                
                DomainException domainEx => (
                    HttpStatusCode.BadRequest,
                    Error.Problem("Domain.Error", domainEx.Message)
                ),
                
                _ => (
                    HttpStatusCode.InternalServerError,
                    Error.Failure("Internal.Error", "An internal server error occurred.")
                )
            };

            // Create ProblemDetails response
            var problemDetails = new
            {
                type = $"https://httpstatuses.com/{(int)statusCode}",
                title = GetTitle(statusCode),
                status = (int)statusCode,
                detail = error.Description,
                instance = context.Request.Path.Value,
                traceId = correlationId,
                errors = error.Code == "Validation" ? GetValidationErrors(exception as Exceptions.ValidationException) : null
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            // Add correlation ID header
            context.Response.Headers.Add("X-Correlation-ID", correlationId);

            var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            await context.Response.WriteAsync(json);
        }

        private static string GetTitle(HttpStatusCode statusCode) => statusCode switch
        {
            HttpStatusCode.BadRequest => "Bad Request",
            HttpStatusCode.NotFound => "Not Found",
            HttpStatusCode.Conflict => "Conflict",
            HttpStatusCode.InternalServerError => "Internal Server Error",
            _ => "Error"
        };

        private static object? GetValidationErrors(Exceptions.ValidationException? validationException)
        {
            if (validationException == null)
                return null;

            return validationException.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );
        }
    }
}
