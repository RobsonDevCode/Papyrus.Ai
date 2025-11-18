using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Papyrus.Domain.Clients.Constants;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Client;

namespace Papyrus.Domain.Clients;

public sealed class PapyrusAiClient : IPapyrusAiClient
{
    private readonly HttpClient _httpClient;

    public PapyrusAiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LlmResponseModel> PromptAsync(string prompt, List<PromptModel>? previousPrompts,
        string? image, CancellationToken cancellationToken)
    {
        var requestBody = BuildRequestBody(prompt, image: image);

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

        var result = await response.Content.ReadFromJsonAsync<LlmResponseModel>(cancellationToken);

        if (result == null)
        {
            throw new InvalidOperationException($"Unable to create note {response.Content} was returned.");
        }

        return result;
    }

    // TODO this method is ugly because of the format Gemini takes json in, might break it up but for now im leaving it 
    private AiRequestModel BuildRequestBody(string prompt, List<PromptModel>? previousPrompts = null,
        string? image = null)
    {
        var requestBody = new AiRequestModel
        {
            SystemInstruction = new SystemInturctions
            {
                Parts =
                [
                    new Part
                    {
                        Text = LlmSettings.Personality
                    }
                ]
            },

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

        if (previousPrompts != null)
        {
            var content = new List<Content>();

            foreach (var previousPrompt in previousPrompts)
            {
                content.Add(new Content
                {
                    Role = RoleConstants.User, 
                    Parts =
                    [
                        new Part
                        {
                            Text = previousPrompt.Prompt
                        }
                    ]
                });
                
                content.Add(new Content
                {
                    Role = RoleConstants.Model,
                    Parts = 
                        [
                            new Part
                            {
                                Text = previousPrompt.Response
                            }
                        ]
                });
            }

            content.Add(new Content
            {
                Parts =
                [
                    new Part
                    {
                        Text = prompt
                    }
                ]
            });
            
            requestBody.Contents = content;
        }
        else
        {
            requestBody.Contents =
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
            ];
        }

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