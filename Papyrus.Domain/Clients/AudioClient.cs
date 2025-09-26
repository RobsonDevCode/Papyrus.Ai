using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Models.Voices;

namespace Papyrus.Domain.Clients;

public sealed class AudioClient : IAudioClient
{
    private readonly HttpClient _client;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AudioClient> _logger;

    public AudioClient(HttpClient client,
        IMemoryCache cache,
        ILogger<AudioClient> logger)
    {
        _client = client;
        _cache = cache;
        _logger = logger;
    }
    
    public Task<Stream> CreateAudioAsync(string voiceId, string text, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<VoiceResponseModel?> GetVoiceAsync(string settingsVoiceId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting voice {id}", settingsVoiceId);
        var voice = await _cache.GetOrCreateAsync(settingsVoiceId, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            
            var response = await _client.GetAsync($"v1/voices/{settingsVoiceId}", cancellationToken);
            
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<VoiceResponseModel>(cancellationToken: cancellationToken);
            
            return result;
        });

        return voice;
    }

    public async Task<VoicesResponseModel> GetVoicesAsync(VoiceSearchModel filters, CancellationToken cancellationToken)
    {
        var query = "v2/voices".ToQueryString(filters);
        var response = await _client.GetAsync(query, cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<VoicesResponseModel>(cancellationToken: cancellationToken);
        if (result is null)
        {
            return new VoicesResponseModel
            {
                Voices = [],
                TotalCount = 0
            };
        }
        
        return result;
    }
}