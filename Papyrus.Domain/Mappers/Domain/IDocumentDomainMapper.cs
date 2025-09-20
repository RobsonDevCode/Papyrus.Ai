using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IDocumentDomainMapper
{
  PagedResponseModel<DocumentPreviewModel> MapToDomain(PagedResponse<DocumentPreview> response, int page, int size);
}