using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IBookmarkPersistenceMapper
{
    Bookmark MapToPersistence(BookmarkModel bookmark);
}