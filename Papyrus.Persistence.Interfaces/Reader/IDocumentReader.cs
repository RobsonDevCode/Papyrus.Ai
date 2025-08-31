using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IDocumentReader
{
    Task<Page?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Page?> GetByGroupIdAsync(Guid documentGroupId, int page, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string documentName, CancellationToken cancellationToken);
    Task<IEnumerable<Page?>> GetPages(Guid documentGroupId, int[] pageNumbers, CancellationToken cancellationToken);
}