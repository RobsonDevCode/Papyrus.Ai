using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IAudioWriter
{
    Task SaveAsync(string s3Key, Stream audioStream, CancellationToken cancellationToken);

    Task SaveAlignmentsAsync(string s3Key, IEnumerable<AlignmentData> alignment, CancellationToken cancellationToken);
}