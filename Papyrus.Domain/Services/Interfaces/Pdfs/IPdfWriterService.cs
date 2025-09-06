namespace Papyrus.Domain.Services.Interfaces;

public interface IPdfWriterService
{
    Task SaveAsync(string s3Key, Stream pdfBytes, CancellationToken cancellationToken);
}