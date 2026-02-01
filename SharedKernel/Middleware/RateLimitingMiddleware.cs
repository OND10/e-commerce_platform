using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace SharedKernel.Middleware;

/// <summary>
/// Rate limiting middleware using in-memory cache for request counting
/// For production with multiple instances, use distributed cache (Redis)
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    // Configuration - these should come from appsettings in production
    private readonly int _requestLimit = 100; // requests per window
    private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(
        RequestDelegate next,
        IMemoryCache cache,
        ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get client identifier (IP address or user ID if authenticated)
        var clientId = GetClientIdentifier(context);
        var cacheKey = $"rate_limit:{clientId}";

        // Get current request count
        var requestCount = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _timeWindow;
            return 0;
        });

        if (requestCount >= _requestLimit)
        {
            _logger.LogWarning(
                "Rate limit exceeded for client {ClientId}. Requests: {RequestCount}/{Limit}",
                clientId, requestCount, _requestLimit);

            await HandleRateLimitExceeded(context);
            return;
        }

        // Increment request count
        _cache.Set(cacheKey, requestCount + 1, _timeWindow);

        // Add rate limit headers
        context.Response.Headers.Add("X-RateLimit-Limit", _requestLimit.ToString());
        context.Response.Headers.Add("X-RateLimit-Remaining", (_requestLimit - requestCount - 1).ToString());
        context.Response.Headers.Add("X-RateLimit-Reset", DateTimeOffset.UtcNow.Add(_timeWindow).ToUnixTimeSeconds().ToString());

        await _next(context);
    }

    private static string GetClientIdentifier(HttpContext context)
    {
        // Try to get user ID if authenticated
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst("sub")?.Value 
                ?? context.User.FindFirst("userId")?.Value 
                ?? context.User.Identity.Name;
            
            if (!string.IsNullOrEmpty(userId))
                return $"user:{userId}";
        }

        // Fall back to IP address
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        // Check for forwarded IP (behind proxy/load balancer)
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            ipAddress = forwardedFor.ToString().Split(',')[0].Trim();
        }

        return $"ip:{ipAddress}";
    }

    private static async Task HandleRateLimitExceeded(HttpContext context)
    {
        var problemDetails = new
        {
            type = "https://httpstatuses.com/429",
            title = "Too Many Requests",
            status = (int)HttpStatusCode.TooManyRequests,
            detail = "Rate limit exceeded. Please try again later.",
            instance = context.Request.Path.Value
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
