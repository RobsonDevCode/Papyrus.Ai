using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IAudioAlignmentPersistenceMapper
{
    AlignmentData MapToPersistence(AlignmentDataModel? alignmentData);
    
    IEnumerable<AlignmentData> MapToPersistence(IEnumerable<AlignmentDataModel?> alignmentData);
}