using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.Audio;

namespace Papyrus.Domain.Mappers.Domain;

public interface IAudioDomainMapper
{
    CreateAudioRequestModel MapToDomain(CreateAudioBookRequest request);
}