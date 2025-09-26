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

    public DocumentResponse[] MapToResponse(DocumentPreviewModel[] documents)
    {
        return documents.Length == 0 ? [] : documents.Select(MapToResponse).ToArray();
    }
}