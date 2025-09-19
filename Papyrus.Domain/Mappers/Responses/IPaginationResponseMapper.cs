using Papyrus.Api.Contracts.Contracts.Api;

namespace Papyrus.Domain.Mappers.Responses;

public interface IPaginationResponseMapper
{
    PagedResponse<T> MapToResponse<T>(List<T> items, int page, int pageSize, int totalCount);
}