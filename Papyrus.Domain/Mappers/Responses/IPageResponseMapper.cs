using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Documents;

namespace Papyrus.Domain.Mappers;

public interface IPageResponseMapper
{
    DocumentPageResponse MapToResponse(PageModel pageModel);
    DocumentPagesResponse MapToResponse(IEnumerable<PageModel> pages, int totalPages);
}