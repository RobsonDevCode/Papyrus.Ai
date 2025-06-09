using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Services.Interfaces.Notes;

namespace Papyrus.Domain.Services.Notes;

public sealed class NoteReaderService : INoteReaderService
{
    public async Task<NoteModel> GetNotesAsync(NoteRequestModel noteRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}