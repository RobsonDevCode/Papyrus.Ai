using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Mappers.Responses;

public interface IAudioSettingsResponseMapper
{
    AudioSettingsResponse MapToResponse(AudioSettingsModel audioSettings);
}