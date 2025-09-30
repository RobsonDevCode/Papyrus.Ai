using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IAudioSettingsDomainMapper
{
    AudioSettingsRequestModel MapToDomain(AudioSettingsRequest request);

    AudioSettingsModel MapToDomain(AudioSettings audioSettings);
}