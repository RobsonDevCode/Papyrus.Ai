namespace Papyrus.Domain.Services.Interfaces;

public interface IPdfReaderService
{
    Task<byte[]> GetPdfBytesAsync(Guid documentGroupId, CancellationToken cancellationToken);
}