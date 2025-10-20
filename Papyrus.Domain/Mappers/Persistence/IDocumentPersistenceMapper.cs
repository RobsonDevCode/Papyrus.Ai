using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IDocumentPersistenceMapper
{
    DocumentPreview MapToPersistence(Page page, int totalPages, Guid userId);
}