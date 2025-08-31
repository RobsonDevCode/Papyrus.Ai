

using Papyrus.Domain.Models;

namespace Papyrus.Domain.Clients;

public interface IPapyrusAiClient
{
    Task<LlmResponseModel> CreateNoteAsync(string prompt, List<PromptModel>? previousPrompts = null,
        string? images = null, CancellationToken cancellationToken = default);
    
}