using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Services.Interfaces.Notes;

public interface INoteReaderService
{
    Task<NoteModel> GetNotesAsync(NoteRequestModel noteRequest, CancellationToken cancellationToken);
}