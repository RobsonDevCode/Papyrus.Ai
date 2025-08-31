using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface IImagePersistenceMapper
{
    Image MapToPersistence(byte[] imageBytes, Guid documentId, string documentName, int pageNumber);
}