using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AudioSettings MapToPersistence(AudioSettingsModel audioSettingsModel)
    {
        return new AudioSettings
        {
            Id = audioSettingsModel.Id,
            VoiceId = audioSettingsModel.VoiceId,
            VoiceSetting = MapToPersistence(audioSettingsModel.VoiceSettings),
        };
    }

    private VoiceSetting MapToPersistence(VoiceSettingModel voiceSettingModel)
    {
        return new VoiceSetting
        {
            Speed = voiceSettingModel.Speed,
            Stability = voiceSettingModel.Stability,
            UseSpeakerBoost = voiceSettingModel.UseSpeakerBoost
        };
    }
}
