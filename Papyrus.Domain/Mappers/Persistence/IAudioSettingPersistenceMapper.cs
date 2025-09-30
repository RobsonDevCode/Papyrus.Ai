using Papyrus.Domain.Models.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IAudioSettingPersistenceMapper
{
    AudioSettings MapToPersistence(AudioSettingsRequestModel audioSettingsRequestModel);
}