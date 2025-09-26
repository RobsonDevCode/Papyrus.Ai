using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Persistance.Interfaces.Contracts;
using VoiceSettings = Papyrus.Api.Contracts.Contracts.VoiceSettings;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public VoiceModel MapToDomain(VoiceResponseModel voice)
    {
        return new VoiceModel
        {
            VoiceId = voice.VoiceId,
            Name = voice.Name,
            Category = voice.Category,
            Description = voice.Description,
            PreviewUrl = voice.PreviewUrl,
            Settings = new VoiceSettings
            {
                Speed = voice.Settings?.Speed ?? 0,
                Stability = voice.Settings?.Stability ?? 0,
                UseSpeakerBoost = voice.Settings?.UseSpeakerBoost ?? false,
            },
            Labels = voice.Labels
        };
    }

    public IEnumerable<VoiceModel> MapToDomain(IEnumerable<VoiceResponseModel> voices)
    {
        return voices.Select(MapToDomain);
    }

    public VoiceModel MapToDomain(Voice voice)
    {
        return new VoiceModel
        {
            VoiceId = voice.VoiceId,
            Name = voice.Name,
            Category = voice.Category,
            Description = voice.Description,
            CreatedAtUnix = voice.CreatedAtUnix ,
            Labels = MapToDomain(voice.Labels),
            Settings = new VoiceSettings
            {
                Speed = voice.Settings?.Speed ?? 1,
                Stability = voice.Settings?.Stability ?? 0.5,
                UseSpeakerBoost = voice.Settings?.UseSpeakerBoost ?? false
            },
            PreviewUrl = voice.PreviewUrl
        };
    }

    public IEnumerable<VoiceModel> MapToDomain(IEnumerable<Voice> voices)
    {
        return voices.Select(MapToDomain);
    }
}