using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.Documents;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public DocumentResponse MapToResponse(DocumentPreviewModel document)
    {
        return new DocumentResponse
        {
            DocumentGroupId = document.DocumentGroupId,
            Name = document.Name,
            FrontCoverImageUrl = document.FrontCoverImageUrl,
            Author = document.Author,
            CreatedAt = document.CreatedAt,
            TotalPages = document.TotalPages
        };
    }

    public List<DocumentResponse> MapToResponse(List<DocumentPreviewModel> documents)
    {
        return documents.Count == 0 ? [] : documents.Select(MapToResponse).ToList();
    }
}