using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Mappers.Domain;

public interface IAudioSettingsDomainMapper
{
    AudioSettingsModel MapToDomain(AudioSettingsRequest request);
}