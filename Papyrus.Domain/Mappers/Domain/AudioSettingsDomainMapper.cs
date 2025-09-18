using Papyrus.Api.Contracts.Contracts;
using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AudioSettingsModel MapToDomain(AudioSettingsRequest request)
    {
        return new AudioSettingsModel
        {
            Id = request.Id,
            VoiceId = request.VoiceId,
            VoiceSettings = MapToDomain(request.VoiceSettings)
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
}