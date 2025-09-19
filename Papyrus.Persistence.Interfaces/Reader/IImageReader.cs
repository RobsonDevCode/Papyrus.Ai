namespace Papyrus.Persistance.Interfaces.Reader;

public interface IImageReader
{
    Task<Stream> GetImageAsStreamAsync(string s3key, CancellationToken cancellationToken); 
}