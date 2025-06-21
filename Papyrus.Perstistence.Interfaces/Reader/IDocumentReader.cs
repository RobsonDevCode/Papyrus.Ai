using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface IDocumentReader
{
    Task<Page?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Page?> GetByGroupIdAsync(Guid documentGroupId, int page, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string documentName, CancellationToken cancellationToken);
}