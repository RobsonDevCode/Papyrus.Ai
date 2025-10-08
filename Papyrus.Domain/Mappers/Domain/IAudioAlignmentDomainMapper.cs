using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IAudioAlignmentDomainMapper
{
    AlignmentDataModel MapToDomain(AlignmentData alignmentData);

    IEnumerable<AlignmentDataModel> MapToDomain(IEnumerable<AlignmentData> alignmentData);
}