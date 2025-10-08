using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Domain.Mappers.Responses;

public interface IAudioAlignmentResponseMapper
{
    AudioWithAlignmentResponse MapToResponse(AudioAlignmentResultModel alignmentResponse);
    
    AlignmentDataResponse MapToResponse(AlignmentDataModel alignmentResponse);
    
    List<AlignmentDataResponse> MapToResponse(List<AlignmentDataModel> alignmentResponses);
}