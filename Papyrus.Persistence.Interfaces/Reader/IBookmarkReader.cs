using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IBookmarkReader
{
    Task<Bookmark?> GetByGroupIdAsync(Guid documentGroupId, CancellationToken cancellationToken);
}