using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentReaderService
{
    Task<PageModel?> GetByIdAsync(Guid pageId, CancellationToken cancellationToken);
    
    Task<PageModel?> GetByGroupIdAsync(Guid documentGroupId, int? pageNumber,
        CancellationToken cancellationToken);
}