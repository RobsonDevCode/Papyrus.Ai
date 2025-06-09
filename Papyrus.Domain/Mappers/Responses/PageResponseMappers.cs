
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Mappers;

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
            DocumentType = pageModel.DocumentType
        };
    }
}