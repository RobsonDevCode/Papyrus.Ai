using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IVoiceDomainMapper
{
    VoiceModel MapToDomain(VoiceResponseModel voice);

    IEnumerable<VoiceModel> MapToDomain(IEnumerable<VoiceResponseModel> voices);

    VoiceModel MapToDomain(Voice voice);
    
    IEnumerable<VoiceModel> MapToDomain(IEnumerable<Voice> voices);
}