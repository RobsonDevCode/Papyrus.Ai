using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper 
{
    public PagedResponseModel<DocumentPreviewModel> MapToDomain(PagedResponse<DocumentPreview> response, int page, int size)
    {
        return new PagedResponseModel<DocumentPreviewModel>
        {
            Items = MapToDomain(response.Data).ToArray(),
            Pagination = new PaginationModel
            {
                Page = page,
                Size = size,
                TotalPages = response.TotalPages
            }
        };
    }

    public DocumentPreviewModel MapToDomain(DocumentPreview preview)
    {
        return new DocumentPreviewModel
        {
            DocumentGroupId = preview.DocumentGroupId,
            Name = preview.Name,
            Author = preview.Author,
            FrontCoverImageUrl = preview.FrontCoverImageUrl,
            CreatedAt = preview.CreatedAt,
            TotalPages = preview.TotalPages
        };
    }

    public IEnumerable<DocumentPreviewModel> MapToDomain(IEnumerable<DocumentPreview> documentPreviews)
    {
        return documentPreviews.Select(MapToDomain);
    }
}