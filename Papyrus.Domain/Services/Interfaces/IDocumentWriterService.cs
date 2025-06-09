using Papyrus.Domain.Models;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentWriterService
{
    Task StoreDocumentAsync(DocumentModel documentModel, CancellationToken cancellationToken);
}