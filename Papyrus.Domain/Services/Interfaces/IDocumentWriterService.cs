using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Documents;

namespace Papyrus.Domain.Services.Interfaces;

public interface IDocumentWriterService
{
    Task StoreDocumentAsync(Guid userId, DocumentModel document, CancellationToken cancellationToken);
}