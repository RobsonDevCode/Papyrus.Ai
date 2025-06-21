
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Clients;

public interface IPapyrusAiClient
{
    Task<LlmResponse> CreateNoteAsync(string prompt,
        string images, CancellationToken cancellationToken);
    
}