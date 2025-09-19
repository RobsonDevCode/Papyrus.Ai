namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IImageWriter
{
    Task SaveAsync(string s3Key, Stream stream, CancellationToken cancellationToken);
}