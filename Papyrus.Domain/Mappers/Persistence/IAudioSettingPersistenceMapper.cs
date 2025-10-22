using Papyrus.Domain.Models.Audio;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IAudioSettingPersistenceMapper
{
    AudioSettings MapToPersistence(AudioSettingsRequestModel audioSettingsRequestModel);
}