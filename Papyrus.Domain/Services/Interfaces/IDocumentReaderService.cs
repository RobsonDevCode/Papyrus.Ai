using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentReaderService
{ 
    Task<(IEnumerable<DocumentPreviewModel> Documents, int TotalCount)> GetDocuments(PaginationRequestModel pagination, CancellationToken cancellationToken);
}