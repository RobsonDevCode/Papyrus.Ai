
namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudiobookWriterService
{
    Task<byte[]> CreateAsync(Guid documentGroupId, string voiceId, int[] p,  CancellationToken cancellationToken);
}