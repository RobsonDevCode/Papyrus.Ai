using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Models.Voices;

namespace Papyrus.Domain.Clients;

public interface IAudioClient
{
    Task<AudioWithAlignmentResult> CreateWithAlignmentAsync(string text, VoiceSettingModel voiceSettings,
        string voiceId,
        CancellationToken cancellationToken);
    
    ValueTask<VoiceResponseModel?> GetVoiceAsync(string settingsVoiceId, CancellationToken cancellationToken);
    Task<VoicesResponseModel> GetVoicesAsync(VoiceSearchModel filters, CancellationToken cancellationToken); 
}