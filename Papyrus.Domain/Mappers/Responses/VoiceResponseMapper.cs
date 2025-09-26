using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public VoiceResponse MapToResponse(VoiceModel voiceModel)
    {
        return new VoiceResponse
        { 
            VoiceId= voiceModel.VoiceId,
            Name = voiceModel.Name,
            Description = voiceModel.Description,
            Category = voiceModel.Category,
            Settings = voiceModel.Settings,
            Labels = MapToResponse(voiceModel.Labels),
            PreviewUrl = voiceModel.PreviewUrl,
            CreatedAtUnix = voiceModel.CreatedAtUnix,
            IsFavourited = voiceModel.IsFavourited,
            IsSelected = voiceModel.IsSelected
        };
    }

    public IEnumerable<VoiceResponse> MapToResponse(IEnumerable<VoiceModel> voiceModels)
    {
        return voiceModels.Select(MapToResponse);
    }
}