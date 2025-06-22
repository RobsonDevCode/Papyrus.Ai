using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public partial class Mapper
{
    public Note MapToPersistence(LlmResponse response, PageModel pageModel)
    {
        return new Note
        {
            DocumentGroupId = pageModel.DocumentGroupId,
            Text = response.Candidates.FirstOrDefault()?.Content.Parts.FirstOrDefault()?.Text, //TODO clean this shit up this a null reference waiting to happen 
            Title = pageModel.DocumentName,
            RelatedPage = pageModel.PageNumber,
            CreatedAt = response.CreatedAt,
            UpdatedAt = pageModel.UpdatedAt
        };
    }
}