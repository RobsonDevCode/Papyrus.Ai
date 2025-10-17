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
            Page = request.Page,
            CreatedAt = request.CreateAt
        };
    }

    public BookmarkModel MapToDomain(UpdateBookmarkRequest request)
    {
        return new BookmarkModel
        {
            Id = request.Id,
            DocumentGroupId = request.DocumentGroupId,
            UserId = request.UserId,
            Page = request.NewPage
        };
    }

    public BookmarkModel MapToDomain(Bookmark bookmark)
    {
        return new BookmarkModel
        {
            Id = bookmark.Id,
            DocumentGroupId = bookmark.DocumentGroupId,
            Page = bookmark.Page,
            CreatedAt = bookmark.CreatedAt
        };
    }
}