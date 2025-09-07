namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioWriter
{
    Task SaveAsync(string s3Key, Stream audioStream, CancellationToken cancellationToken);
}