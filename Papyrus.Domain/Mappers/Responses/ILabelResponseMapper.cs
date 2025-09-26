using Papyrus.Api.Contracts.Contracts;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Mappers.Responses;

public interface ILabelResponseMapper
{
    LabelsResponse? MapToResponse(Labels? labels);
}