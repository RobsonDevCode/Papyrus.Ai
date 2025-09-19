using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public DocumentPreview MapToPersistence(Page page, int totalPages)
    {
        return new DocumentPreview
        {
            DocumentGroupId = page.DocumentGroupId,
            Name = page.DocumentName,
            Author = page.Author,
            FrontCoverImageUrl = page.ImageUrl,
            TotalPages = totalPages,
            CreatedAt = page.CreatedAt,
        };
    }
}
