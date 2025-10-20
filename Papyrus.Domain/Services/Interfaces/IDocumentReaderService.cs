using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentReaderService
{ 
    Task<PagedResponseModel<DocumentPreviewModel>> GetDocuments(Guid userId, string? searchTerm, PaginationRequestModel pagination, CancellationToken cancellationToken);
}