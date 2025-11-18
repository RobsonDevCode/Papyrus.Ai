using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Explanation;

namespace Papyrus.Domain.Mappers.Domain;

public interface IExplanationDomainMapper
{
    CreateExplanationRequestModel MapToDomain(CreateExplanationRequest request, Guid userId);
}