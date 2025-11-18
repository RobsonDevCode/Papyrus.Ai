using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public BookmarkModel MapToDomain(CreateBookmarkRequest request)
    {
        return new BookmarkModel
        {
            Id = request.Id ?? Guid.Empty,
            DocumentGroupId = request.DocumentGroupId,
            UserId = request.UserId,
            Timestamp = request.TimeStamp,
            Page = request.Page,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public BookmarkModel MapToDomain(Bookmark bookmark)
    {
        return new BookmarkModel
        {
            Id = bookmark.Id,
            UserId = bookmark.UserId,
            DocumentGroupId = bookmark.DocumentGroupId,
            Timestamp = bookmark.Timestamp,
            Page = bookmark.Page,
            CreatedAt = bookmark.CreatedAt,
        };
    }
}