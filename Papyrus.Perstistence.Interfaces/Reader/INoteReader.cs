using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface INoteReader
{
    Task<Note?> GetNoteAsync(Guid id, CancellationToken cancellationToken); 
    Task<PagedResponse<Note?>> GetPagedNotesAsync(int pageNumber, int size, int? pdfPage, CancellationToken cancellationToken);
}