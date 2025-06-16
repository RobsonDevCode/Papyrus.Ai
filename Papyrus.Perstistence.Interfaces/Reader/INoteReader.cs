using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface INoteReader
{
    Task<Note?> GetNoteAsync(Guid id, CancellationToken cancellationToken); 
    Task<PagedResponse<Note>> GetPagedNotesAsync(Guid documentId, PaginationOptions options, CancellationToken cancellationToken);
    Task<List<Note>> GetNotesOnPageAsync(Guid documentGroupId, int pdfPage, CancellationToken cancellationToken);
}