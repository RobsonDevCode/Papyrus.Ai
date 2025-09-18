using System.Net.Http.Headers;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Protocols.Configuration;
using Papyrus.Ai.Configuration;
using Papyrus.Domain.Clients;
using Polly;

namespace Papyrus.Ai.Extensions;

public static class HttpClientExtensions
{
    public static void AddExternalHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPapyrusAiClient(configuration);
        services.AddElevenLabsClient(configuration);
    }
    
    private static void AddPapyrusAiClient(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("PapyrusAiClient").Get<PapyrusAiClientSettings>();
        if (options == null)
        {
            throw new InvalidConfigurationException("PapyrusAiClientSettings is missing from configuration.");
        }

        services.AddHttpClient<IPapyrusAiClient, PapyrusAiClient>(client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var baseUrl = options.BaseUrl + options.ApiKey;
            client.BaseAddress = new Uri(baseUrl);
        })
        .AddResilienceHandler("Ai", pipeline =>
        {
            pipeline.AddTimeout(TimeSpan.FromMinutes(4)); //we have a long timeout because local LLMS are slow asf

            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromMilliseconds(500)
            });

            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromMinutes(5),
                FailureRatio = 0.9,
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(5)
            });
        });
    }

    private static void AddElevenLabsClient(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("ElevenLabsClient").Get<ElevenLabsClientSettings>();
        if (options == null)
        {
            throw new InvalidConfigurationException("ElevenLabsClient is missing from configuration.");
        }

        services.AddHttpClient<IAudioClient, AudioClient>(client =>
            {
                client.DefaultRequestHeaders.Add("xi-api-key", options.ApiKey);
                client.BaseAddress = options.BaseUrl;
            })
            .AddResilienceHandler("ElevenLabs", pipeline =>
            {
                pipeline.AddTimeout(TimeSpan.FromMinutes(1)); 

                pipeline.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    Delay = TimeSpan.FromMilliseconds(500)
                });

                pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    SamplingDuration = TimeSpan.FromMinutes(2),
                    FailureRatio = 0.9,
                    MinimumThroughput = 3,
                    BreakDuration = TimeSpan.FromSeconds(5)
                });
            });
    }
}