using Papyrus.Api.Contracts.Contracts;
using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AudioSettingsResponse MapToResponse(AudioSettingsModel audioSettingsModel)
    {
        return new AudioSettingsResponse
        {
            Id = audioSettingsModel.Id,
            VoiceId = audioSettingsModel.VoiceId,
            VoiceSettings = MapToResponse(audioSettingsModel.VoiceSettings),
            CreatedAt = audioSettingsModel.CreatedAt,
            UpdatedAt = audioSettingsModel.UpdatedAt,
        };
    }

    public VoiceSettings MapToResponse(VoiceSettingModel voiceSettings)
    {
        return new VoiceSettings
        {
            Speed = voiceSettings.Speed,
            Stability = voiceSettings.Stability,
            UseSpeakerBoost = voiceSettings.UseSpeakerBoost,
        };
    }
}