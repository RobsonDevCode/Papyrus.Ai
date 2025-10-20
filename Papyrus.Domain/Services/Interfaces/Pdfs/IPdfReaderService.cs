namespace Papyrus.Domain.Services.Interfaces;

public interface IPdfReaderService
{
    Task<byte[]> GetPdfBytesAsync(Guid userId, Guid documentGroupId, CancellationToken cancellationToken);
}