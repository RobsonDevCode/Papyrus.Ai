namespace Papyrus.Domain.Models.Client.Audio;

public interface IAudioClient
{
    Task<Stream> CreateAudioAsync(string voiceId, string text, CancellationToken? cancellationToken);
}