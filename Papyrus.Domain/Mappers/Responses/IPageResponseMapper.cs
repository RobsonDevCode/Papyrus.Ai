using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Mappers;

public interface IPageResponseMapper
{
    DocumentPageResponse MapToResponse(PageModel pageModel);
}