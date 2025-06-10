using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface IDocumentReader
{
    Task<Page?> GetPageById(Guid documentGroupId, int page, CancellationToken cancellationToken);
    Task<string?> GetNameById(Guid documentGroupId, CancellationToken cancellationToken);
}