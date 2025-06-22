using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Clients.Constants;
using Papyrus.Domain.Models.Client;

namespace Papyrus.Domain.Clients;

public sealed class PapyrusAiClient : IPapyrusAiClient
{
    private readonly HttpClient _httpClient;

    public PapyrusAiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LlmResponse> CreateNoteAsync(string prompt,
        string? image, CancellationToken cancellationToken)
    {
        var requestBody = BuildRequestBody(prompt, image);

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync("", content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(cancellationToken: cancellationToken);
            if (error == null)
            {
                throw new HttpRequestException($"Failed to create note: {json}");
            }

            throw new HttpRequestException(
                $"Received response {response.StatusCode}  with error message: {error.Message}");
        }

        var result = await response.Content.ReadFromJsonAsync<LlmResponse>(cancellationToken);

        if (result == null)
        {
            throw new InvalidOperationException($"Unable to create note {response.Content} was returned.");
        }

        return result;
    }

    private AiRequestModel BuildRequestBody(string prompt, string? image = null)
    {
        var requestBody = new AiRequestModel
        {
            Contents =
            [
                new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = prompt
                        }
                    ]
                }
            ],
            GenerationConfig = new GenerationConfig
            {
                Temperature = LlmSettings.Temperature,
                TopK = LlmSettings.TopK,
                TopP = LlmSettings.TopP,
                ThinkingConfig = new ThinkingConfig
                {
                    IncludeThoughts = false,
                    ThinkingBudget = LlmSettings.ThinkingBudget
                }
            }
        };
        
        if (!string.IsNullOrWhiteSpace(image))
        {
            requestBody.Contents[0].Parts.Add(new Part
            {
                InlineData = new InlineData
                {
                    Data = image,
                    MimeType = "image/png"
                }
            });
        }
        
        return requestBody;
    }
}