using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IPageReader
{
    Task<Page?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    [Obsolete("Moving over to user based libraries will be removed in a future release.")]
    Task<Page?> GetByGroupIdAsync(Guid documentGroupId, Guid userId, int page, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string documentName, Guid userId, CancellationToken cancellationToken);

    Task<(IEnumerable<Page?> Pages, int TotalPages)> GetPages(Guid documentGroupId, Guid userId, int[] pageNumbers,
        CancellationToken cancellationToken);
}