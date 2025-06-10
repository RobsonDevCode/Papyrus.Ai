using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentReaderService
{
    Task<PageModel?> GetPageByIdAsync(Guid documentGroupId, int? page,
        CancellationToken cancellationToken);

    ValueTask<string?> GetDocumentNameAsync(Guid documentGroupId, CancellationToken cancellationToken);
}