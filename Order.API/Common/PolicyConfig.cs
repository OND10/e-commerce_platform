// File: Order.API/Common/PolicyConfig.cs
using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Order.API.Common
{
    public static class PolicyConfig
    {
        // Retry on 5xx or 408 with exponential back‑off: 2^retryAttempt seconds
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        // optional: log each retry attempt
                        Console.WriteLine(
                            $"[RetryPolicy] Retry {retryAttempt} after {timespan.TotalSeconds}s due to " +
                            $"{(outcome.Exception != null ? outcome.Exception.Message : outcome.Result.StatusCode.ToString())}"
                        );
                    }
                );

        // Circuit‑breaker: breaks after 5 consecutive faults, stays open for 30s
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, breakDelay) =>
                    {
                        Console.WriteLine(
                            $"[CircuitBreaker] Open for {breakDelay.TotalSeconds}s due to " +
                            $"{(outcome.Exception != null ? outcome.Exception.Message : outcome.Result.StatusCode.ToString())}"
                        );
                    },
                    onReset: () => Console.WriteLine("[CircuitBreaker] Reset; calls will flow again."),
                    onHalfOpen: () => Console.WriteLine("[CircuitBreaker] Half‑open; next call is a trial.")
                );
    }
}
