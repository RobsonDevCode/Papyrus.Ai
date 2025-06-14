using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
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

    public async Task<OllamaResponse> CreateNoteAsync(string book, string prompt,
        CancellationToken cancellationToken)
    {
        var requestBody = new AiRequestModel
        {
            Model = _model,
            Stream = false,
            Options = new AiRequestOptionsModel
            {
                Tempurature = 0.1,
                TopP = 0.9
            }
        };

        requestBody.Prompt = !string.IsNullOrWhiteSpace(book)
            ? $"Create detailed notes on this text using numbered or normal bullet points from {book}. Add relevant context and supplementary information where helpful. Provide only the notes without commentary. {prompt}"
            : $"Create comprehensive notes on the following text using numbered or normal bullet points. Expand with relevant additional information where appropriate. Provide only the notes without any introductory or concluding remarks. {prompt}";
        
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync("api/generate", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken);

        if (result == null)
        {
            throw new InvalidOperationException($"Unable to create note {response.Content} was returned.");
        }

        return result;
    }
}