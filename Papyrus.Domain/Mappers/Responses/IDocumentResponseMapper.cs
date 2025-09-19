using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.Documents;

namespace Papyrus.Domain.Mappers.Responses;

public interface IDocumentResponseMapper
{
    DocumentResponse MapToResponse(DocumentPreviewModel document);

    List<DocumentResponse> MapToResponse(List<DocumentPreviewModel> documents);
}