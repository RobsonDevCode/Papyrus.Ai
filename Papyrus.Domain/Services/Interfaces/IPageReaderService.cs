using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Services.Interfaces;

public interface IPageReaderService
{
    Task<PageModel?> GetPageByIdAsync(Guid pageId, CancellationToken cancellationToken);
    
    Task<PageModel?> GetByGroupIdAsync(Guid documentGroupId, int? pageNumber,
        CancellationToken cancellationToken);

    Task<(IEnumerable<PageModel> Pages, int TotalPages)> GetPages(Guid documentGroupId, int[] pageNumbers,
        CancellationToken cancellationToken);
    
}