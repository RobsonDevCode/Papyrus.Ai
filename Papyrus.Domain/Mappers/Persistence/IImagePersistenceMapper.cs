using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface IImagePersistenceMapper
{
    Image MapToPersistence(Guid documentId, string documentName, int pageNumber, string s3Key);
}