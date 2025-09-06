namespace Papyrus.Domain.Models.Client.Audio;

public interface IAudioClient
{
    Task<AudioResponse> CreateAudioAsync(AudioRequest request, CancellationToken cancellationToken);
}