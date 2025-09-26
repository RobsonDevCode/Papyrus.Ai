using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Models.Voices;

namespace Papyrus.Domain.Clients;

public interface IAudioClient
{
    Task<Stream> CreateAudioAsync(string voiceId, string text, CancellationToken cancellationToken);
    ValueTask<VoiceResponseModel?> GetVoiceAsync(string settingsVoiceId, CancellationToken cancellationToken);
    Task<VoicesResponseModel> GetVoicesAsync(VoiceSearchModel filters, CancellationToken cancellationToken); 
}