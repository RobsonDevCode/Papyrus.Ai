using Papyrus.Domain.Models.Documents;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public PageModel? MapToDomain(Page? page)
    {
        if (page == null)
        {
            return null;
        }
        return new PageModel
        {
            DocumentGroupId = page.DocumentGroupId,
            DocumentName = page.DocumentName,
            UserId = page.UserId,
            S3Key = page.S3Key,
            Content = page.Content,
            PageNumber = page.PageNumber,
            DocumentType = page.Type,
            ImageCount = page.ImageCount,
            ImageUrl = page.ImageUrl,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            Author = page.Author
        };
    }

    public IEnumerable<PageModel?> MapToDomain(IEnumerable<Page?> pages)
    {
        return pages.Select(MapToDomain);
    }
}