
namespace Papyrus.Domain.Clients;

public interface IPapyrusAiClient
{
    Task<OllamaResponse> CreateNoteAsync(string book, string text,
        CancellationToken cancellationToken);

 
}