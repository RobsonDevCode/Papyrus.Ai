using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public partial class Mapper
{
    public PageModel MapToDomain(Page page)
    {
        return new PageModel
        {
            DocumentGroupId = page.DocumentGroupId,
            DocumentName = page.DocumentName,
            Content = page.Content,
            PageNumber = page.PageNumber,
            DocumentType = page.DocumentType,
            Images = page.Images?.Select(MapToDomain).ToList() ?? [],
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            Author = page.Author
        };
    }
    
    
}