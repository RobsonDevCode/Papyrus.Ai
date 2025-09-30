using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Contracts;
using VoiceSettings = Papyrus.Api.Contracts.Contracts.VoiceSettings;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AudioSettingsRequestModel MapToDomain(AudioSettingsRequest request)
    {
        return new AudioSettingsRequestModel
        {
            Id = request.Id,
            VoiceId = request.VoiceId,
            VoiceSettings = MapToDomain(request.VoiceSettings)
        };
    }

    public AudioSettingsModel MapToDomain(AudioSettings audioSettings)
    {
        return new AudioSettingsModel
        {
            Id = audioSettings.Id,
            VoiceId = audioSettings.VoiceId,
            VoiceSettings = MapToDomain(audioSettings.VoiceSetting),
            CreatedAt = audioSettings.CreatedAt,
            UpdatedAt = audioSettings.UpdatedAt,
        };
    }

    private VoiceSettingModel MapToDomain(VoiceSettings vs)
    {
        return new VoiceSettingModel
        {
            Stability = vs.Stability,
            UseSpeakerBoost = vs.UseSpeakerBoost,
            Speed = vs.Speed
        };
    }
    
    private VoiceSettingModel MapToDomain(VoiceSetting vs)
    {
        return new VoiceSettingModel
        {
            Stability = vs.Stability,
            UseSpeakerBoost = vs.UseSpeakerBoost,
            Speed = vs.Speed
        };
    }
    
}