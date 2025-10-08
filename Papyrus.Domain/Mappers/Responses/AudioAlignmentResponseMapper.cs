using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.Client.Audio;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AudioWithAlignmentResponse MapToResponse(AudioAlignmentResultModel alignmentResponse)
    {
        return new AudioWithAlignmentResponse
        {
            AudioUrl = alignmentResponse.AudioUrl,
            Alignment = MapToResponse(alignmentResponse.Alignment.ToList())
        };
    }

    public AlignmentDataResponse MapToResponse(AlignmentDataModel alignmentResponse)
    {
        return new AlignmentDataResponse
        {
            Characters = alignmentResponse.Charaters,
            CharactersStartTimesSeconds = alignmentResponse.CharacterStartTimesSeconds,
            CharactersEndTimesSeconds = alignmentResponse.CharacterEndTimesSeconds,
        };
    }

    public List<AlignmentDataResponse> MapToResponse(List<AlignmentDataModel> alignmentResponses)
    {
        return alignmentResponses.Select(MapToResponse).ToList();
    }
}