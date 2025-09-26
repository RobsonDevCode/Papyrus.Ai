using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Models.Voices;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts.Filters;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IVoicePersistenceMapper
{
    Voice MapToPersistence(VoiceResponseModel voiceSearchModel);
    
    IEnumerable<Voice> MapToPersistence(IEnumerable<VoiceResponseModel> voiceSearchModels);

    VoiceSearch MapToPersistence(VoiceSearchModel filter);
}