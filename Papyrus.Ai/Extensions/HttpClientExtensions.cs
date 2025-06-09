using System.Net.Http.Headers;
using Microsoft.IdentityModel.Protocols.Configuration;
using Papyrus.Ai.Configuration;
using Papyrus.Domain.Clients;

namespace Papyrus.Ai.Extensions;

public static class HttpClientExtensions
{
    public static void AddPapyrusAiClient(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("PapyrusAiClient").Get<PapyrusAiClientSettings>();
        if (options == null)
        {
            throw new InvalidConfigurationException("PapyrusAiClientSettings is missing from configuration.");
        }

        services.AddHttpClient<IPapyrusAiClient, PapyrusAiClient>(client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = options.BaseUrl;
        })
        .AddStandardResilienceHandler();
    }
}