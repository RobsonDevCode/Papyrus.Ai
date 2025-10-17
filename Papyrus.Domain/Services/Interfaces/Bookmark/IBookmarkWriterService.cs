using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces.Bookmark;

public interface IBookmarkWriterService
{
    Task UpsertAsync(BookmarkModel bookmark, CancellationToken cancellationToken);
}