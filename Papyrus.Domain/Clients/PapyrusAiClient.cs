using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Client;

namespace Papyrus.Domain.Clients;

public sealed class PapyrusAiClient : IPapyrusAiClient
{
    private readonly HttpClient _httpClient;
    private const string _model = "gemma2:2b";
    private readonly ILogger<PapyrusAiClient> _logger;

    public PapyrusAiClient(HttpClient httpClient, ILogger<PapyrusAiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LlmResponse> CreateNoteAsync(string prompt,
       string images, CancellationToken cancellationToken)
    {
        var requestBody = new AiRequestModel
        {
            Model = _model,
            Prompt = prompt,
            Stream = false,
            Options = new AiRequestOptionsModel
            {
                Tempurature = 0.1,
                TopP = 0.9
            }
        };
        
      
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync("api/generate", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LlmResponse>(cancellationToken);

        if (result == null)
        {
            throw new InvalidOperationException($"Unable to create note {response.Content} was returned.");
        }

        return result;
    }
}