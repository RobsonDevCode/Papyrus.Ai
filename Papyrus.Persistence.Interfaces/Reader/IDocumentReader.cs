using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IDocumentReader
{
    Task<PagedResponse<DocumentPreview>> GetPagedDocumentsAsync(string? searchTerm, PaginationOptions pagination, CancellationToken cancellationToken);
}