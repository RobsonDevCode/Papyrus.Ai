using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AlignmentDataModel MapToDomain(AlignmentData alignmentData)
    {
        return new AlignmentDataModel
        {
            Charaters = alignmentData.Charaters,
            CharacterStartTimesSeconds = alignmentData.CharacterStartTimesSeconds,
            CharacterEndTimesSeconds = alignmentData.CharacterEndTimesSeconds
        };
    }

    public IEnumerable<AlignmentDataModel> MapToDomain(IEnumerable<AlignmentData> alignmentData)
    {
        return alignmentData.Select(MapToDomain);
    }
}