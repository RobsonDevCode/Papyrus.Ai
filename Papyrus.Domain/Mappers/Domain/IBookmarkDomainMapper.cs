using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IBookmarkDomainMapper
{
    BookmarkModel MapToDomain(CreateBookmarkRequest request);

    BookmarkModel MapToDomain(Bookmark bookmark);
}