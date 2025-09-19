using Papyrus.Domain.Clients;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Documents;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public Note MapToPersistence(LlmResponseModel responseModel, PageModel pageModel)
    {
        return new Note
        {
            DocumentGroupId = pageModel.DocumentGroupId,
            Text = responseModel.ExtractResponse(),  
            Title = pageModel.DocumentName,
            RelatedPage = pageModel.PageNumber,
            CreatedAt = responseModel.CreatedAt,
            UpdatedAt = responseModel.UpdatedAt,
            ChatId = pageModel.ChatId
        };
    }
}