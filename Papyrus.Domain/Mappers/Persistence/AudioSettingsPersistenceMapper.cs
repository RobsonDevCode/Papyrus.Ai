using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AudioSettings MapToPersistence(AudioSettingsRequestModel audioSettingsRequestModel)
    {
        return new AudioSettings
        {
            Id = audioSettingsRequestModel.Id,
            VoiceId = audioSettingsRequestModel.VoiceId,
            VoiceSetting = MapToPersistence(audioSettingsRequestModel.VoiceSettings),
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
