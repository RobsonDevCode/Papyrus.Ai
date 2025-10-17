using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Services.Interfaces;

public interface IPageReaderService
{
    Task<PageModel?> GetPageByIdAsync(Guid pageId, CancellationToken cancellationToken);

    [Obsolete("Moving over to user based libraries will be removed in a future release.")]
    Task<PageModel?> GetByGroupIdAsync(Guid documentGroupId, int? pageNumber,
        CancellationToken cancellationToken);

    Task<PageModel?> GetByGroupIdAsync(Guid documentGroupId, Guid userId, int? pageNumber,
        CancellationToken cancellationToken);

    Task<(IEnumerable<PageModel> Pages, int TotalPages)> GetPages(Guid documentGroupId, int[] pageNumbers,
        CancellationToken cancellationToken);
}