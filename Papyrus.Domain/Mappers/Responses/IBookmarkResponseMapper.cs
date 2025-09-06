using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Mappers.Responses;

public interface IBookmarkResponseMapper
{
    BookmarkResponse MapToResponse(BookmarkModel bookmark);
}