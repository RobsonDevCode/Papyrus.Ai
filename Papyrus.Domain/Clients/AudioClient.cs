using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Models.Voices;
using Papyrus.Persistance.Interfaces.Contracts;

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
    
    public async Task<Stream> CreateAudioAsync(CreateAudioBookRequestModel bookRequestModel, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating audio for {id}", bookRequestModel.DocumentGroupId);
        var payload = JsonSerializer.Serialize(new CreateTextToSpeechModel
        {
            Text = bookRequestModel.Text,
            VoiceSettings = bookRequestModel.VoiceSettings
        });
        
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"v1/text-to-speech/{bookRequestModel.VoiceId}/stream", content, cancellationToken); 
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    public async Task<AudioWithAlignmentResult> CreateWithAlignmentAsync(string text, VoiceSettingModel voiceSettings,
        string voiceId,
        CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(new CreateTextToSpeechModel
        {
            Text = text,
            VoiceSettings = voiceSettings
        });
        
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"v1/text-to-speech/{voiceId}/stream/with-timestamps",
            content,
            cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken); 
        using var reader = new StreamReader(responseStream);

        var audioChunks = new List<byte[]>();
        var alignments = new List<AlignmentDataModel>();

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            if(string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                using var jsonDoc = JsonDocument.Parse(line);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("audio_base64", out var audioBase64))
                {
                    var base64AudioString = audioBase64.GetString();
                    if (string.IsNullOrWhiteSpace(base64AudioString))
                    {
                        continue;
                    }
                    var audioBytes = Convert.FromBase64String(base64AudioString);
                    audioChunks.Add(audioBytes);
                }

                if (root.TryGetProperty("normalized_alignment", out var alignment) &&
                    alignment.ValueKind != JsonValueKind.Null)
                {
                    alignments.Add(alignment.ParseAlignment());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse audio");
            }
        }
        
        var combinedAudio = audioChunks.CombineAudioChunks();
        var audioStream = new MemoryStream(combinedAudio);
        audioStream.Position = 0;
        
        return new AudioWithAlignmentResult
        {
            AudioStream = audioStream,
            NormalizedAlignment = alignments
        };
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