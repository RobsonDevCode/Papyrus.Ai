using Papyrus.Domain.Clients;
using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public partial class Mapper
{
    public Note MapToPersistance(LlmResponse response, PageModel pageModel)
    {
        return new Note
        {
            DocumentGroupId = pageModel.DocumentGroupId,
            Text = response.Repsonse,
            Title = pageModel.DocumentName,
            RelatedPage = pageModel.PageNumber,
            CreatedAt = response.CreatedAt,
            UpdatedAt = pageModel.UpdatedAt
        };
    }
}