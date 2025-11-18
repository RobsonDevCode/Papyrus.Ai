using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Explanation;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public CreateExplanationRequestModel MapToDomain(CreateExplanationRequest request, Guid userId)
    {
        return new CreateExplanationRequestModel
        {
            DocumentGroupId = request.DocumentGroupId,
            PageNumber = request.PageNumber,
            TextToExplain = request.TextToExplain,
            UserId = userId
        };
    }
}