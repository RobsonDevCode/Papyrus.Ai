using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentReaderService
{
    Task<PageModel?> GetByIdAsync(Guid pageId, CancellationToken cancellationToken);
    
    Task<PageModel?> GetByGroupIdAsync(Guid documentGroupId, int? pageNumber,
        CancellationToken cancellationToken);

    Task<(IEnumerable<PageModel> Pages, int TotalPages)> GetPages(Guid documentGroupId, int[] pageNumbers,
        CancellationToken cancellationToken);
}