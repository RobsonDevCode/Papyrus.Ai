using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces.Bookmark;

public interface IBookmarkWriterService
{
    Task Create(BookmarkModel bookmark, CancellationToken cancellationToken);
}