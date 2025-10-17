using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public Bookmark MapToPersistence(BookmarkModel bookmark)
    {
        return new Bookmark
        {
            Id = bookmark.Id,
            DocumentGroupId = bookmark.DocumentGroupId,
            UserId = bookmark.UserId,
            Page = bookmark.Page,
            CreatedAt = bookmark.CreatedAt
        };
    }
}