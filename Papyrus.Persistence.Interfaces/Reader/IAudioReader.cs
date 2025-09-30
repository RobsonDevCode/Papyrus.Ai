namespace Papyrus.Persistence.S3Bucket;

public interface IAudioReader
{
    Task<Stream?> GetAudioAsync(string s3Key, CancellationToken cancellationToken);
}