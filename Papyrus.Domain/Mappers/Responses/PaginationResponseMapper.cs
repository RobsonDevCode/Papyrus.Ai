using Papyrus.Api.Contracts.Contracts.Api;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public PagedResponse<T> MapToResponse<T>(T[] items, int page, int pageSize, int totalCount)
    {
        return new PagedResponse<T>
        {
            Items = items,
            Pagination = new Pagination(page, pageSize, totalCount)
        };
    }
}