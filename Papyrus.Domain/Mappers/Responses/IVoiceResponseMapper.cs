using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models;

namespace Papyrus.Domain.Mappers.Responses;

public interface IVoiceResponseMapper
{
    VoiceResponse MapToResponse(VoiceModel voiceModel);
    
    IEnumerable<VoiceResponse> MapToResponse(IEnumerable<VoiceModel> voiceModels);
}