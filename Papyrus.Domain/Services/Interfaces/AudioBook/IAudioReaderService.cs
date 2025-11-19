namespace Papyrus.Domain.Services.Interfaces.AudioBook;

public interface IAudioReaderService
{
    Task<Stream?> GetAsync(Guid userId,Guid documentGroupId, string voiceId, int[] pageNumbers, CancellationToken cancellationToken);
    
    Task<Stream?> GetAsync(Guid userId, Guid explanationId, CancellationToken cancellationToken);
}