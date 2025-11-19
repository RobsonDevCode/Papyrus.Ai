using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public CreateAudioBookRequestModel MapToDomain(CreateAudioBookRequest request)
    {
        return new CreateAudioBookRequestModel
        {
            DocumentGroupId = request.DocumentGroupId,
            UserId = request.UserId,
            VoiceId = request.VoiceId,
            Pages = request.Pages,
            Text = request.Text,
            VoiceSettings = MapToDomain(request.Settings)
        };
    }

    public CreateTextToSpeechRequestModel MapToDomain(CreateExplanationTextToSpeechRequest request)
    {
        return new CreateTextToSpeechRequestModel
        {
            Text = request.Text,
            Id = request.Id,
            VoiceId = request.VoiceId,
            VoiceSettings = MapToDomain(request.VoiceSettings) 
        };
    }
}