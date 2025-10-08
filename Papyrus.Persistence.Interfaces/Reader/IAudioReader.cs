
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistence.S3Bucket;

public interface IAudioReader
{
    Task<Stream?> GetAudioAsync(string s3Key, CancellationToken cancellationToken);

    Task<IEnumerable<AlignmentData?>> GetAlignmentInformationAsync(string s3Key, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(string s3Key, CancellationToken cancellationToken);
}