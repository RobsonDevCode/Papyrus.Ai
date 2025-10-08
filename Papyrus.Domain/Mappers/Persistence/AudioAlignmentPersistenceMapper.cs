using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public AlignmentData MapToPersistence(AlignmentDataModel alignmentData)
    {
        return new AlignmentData
        {
            Charaters = alignmentData.Charaters,
            CharacterStartTimesSeconds = alignmentData.CharacterStartTimesSeconds,
            CharacterEndTimesSeconds = alignmentData.CharacterEndTimesSeconds
        };
    }

    public IEnumerable<AlignmentData> MapToPersistence(IEnumerable<AlignmentDataModel?> alignmentData)
    {
        return alignmentData.Select(MapToPersistence);
    }
}