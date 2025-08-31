
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public DocumentPageResponse MapToResponse(PageModel pageModel)
    {
        return new DocumentPageResponse
        {
            DocumentGroupId = pageModel.DocumentGroupId,
            DocumentName = pageModel.DocumentName,
            Content = pageModel.Content,
            PageNumber = pageModel.PageNumber,
            CreatedAt = pageModel.CreatedAt,
            UpdatedAt = pageModel.UpdatedAt,
            Author = pageModel.Author,
            DocumentType = pageModel.DocumentType,
            ImageCount = pageModel.ImageCount,
            ImageUrl = pageModel.ImageUrl
        };
    }

    public IEnumerable<DocumentPageResponse> MapToResponse(IEnumerable<PageModel> pages)
    {
        return pages.Select(MapToResponse);
    }

    public DocumentPagesResponse MapToResponse(IEnumerable<PageModel> pages, int totalPages)
    {
        var mappedPages = pages.Select(MapToResponse);
        return new DocumentPagesResponse
        {
            Pages = mappedPages,
            TotalPages = totalPages
        };
    }
}