
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Clients;

public interface IPapyrusAiClient
{
    Task<OllamaResponse> CreateNoteAsync(string book, string prompt,
        List<ImageModel>? images, CancellationToken cancellationToken);
}