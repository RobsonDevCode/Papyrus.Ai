using System.Threading.RateLimiting;
using Papyrus.Ai.Constants;

namespace Papyrus.Ai.Extensions;

public static class RateLimitingExtensions
{
    public static void AddPapyrusRateLimiting(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRateLimiter(options =>
        {
            options.AddPolicy(RateLimitPolicyConstants.IpPolicy, context =>
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ipAddress,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.AddPolicy(RateLimitPolicyConstants.FixedWindowLimitPolicy, context =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User.Identity?.Name?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

        });
    }
}