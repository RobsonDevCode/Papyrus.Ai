using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public BookmarkResponse MapToResponse(BookmarkModel bookmark)
    {
        return new BookmarkResponse
        {
            Id = bookmark.Id,
            DocumentGroupId = bookmark.DocumentGroupId,
            Page = bookmark.Page,
            CreateAt = bookmark.CreatedAt
        };
    }
}