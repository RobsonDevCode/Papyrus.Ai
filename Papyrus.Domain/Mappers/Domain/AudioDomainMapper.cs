using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public CreateAudioRequestModel MapToDomain(CreateAudioBookRequest request)
    {
        return new CreateAudioRequestModel
        {
            DocumentGroupId = request.DocumentGroupId,
            VoiceId = request.VoiceId,
            Pages = request.Pages,
            Text = request.Text,
            VoiceSettings = MapToDomain(request.Settings)
        };
    }
}