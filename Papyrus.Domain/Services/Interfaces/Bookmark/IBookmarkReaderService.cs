using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces.Bookmark;

public interface IBookmarkReaderService
{
    Task<BookmarkModel?> GetByGroupIdAsync(Guid documentGroupId, CancellationToken cancellationToken);
}