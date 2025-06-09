using Papyrus.Domain.Models;

namespace Papyrus.Domain.Clients;

public interface IPapyrusAiClient
{
    Task<NoteModel> CreateNoteAsync(string book, string text, CancellationToken cancellationToken);
}