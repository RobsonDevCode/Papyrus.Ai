using Papyrus.Domain.Models;

namespace Papyrus.Domain.Clients;

public sealed class PapyrusAiClient : IPapyrusAiClient
{
    private readonly HttpClient _httpClient;

    public PapyrusAiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<NoteModel> CreateNoteAsync(string book, string text, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}