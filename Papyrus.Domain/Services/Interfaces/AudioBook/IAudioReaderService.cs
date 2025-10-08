namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudioReaderService
{
    Task<Stream?> GetAsync(Guid documentGroupId, string voiceId, int[] pageNumbers, CancellationToken cancellationToken);
}