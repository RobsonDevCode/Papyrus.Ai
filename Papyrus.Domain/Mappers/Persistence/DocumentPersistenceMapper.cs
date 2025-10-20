using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public DocumentPreview MapToPersistence(Page page, int totalPages, Guid userId)
    {
        return new DocumentPreview
        {
            DocumentGroupId = page.DocumentGroupId,
            UserId = userId,
            Name = page.DocumentName,
            Author = page.Author,
            FrontCoverImageUrl = page.ImageUrl,
            TotalPages = totalPages,
            CreatedAt = page.CreatedAt,
        };
    }
}
