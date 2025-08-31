using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;
public partial class Mapper 
{
    public Image MapToPersistence(byte[] imageBytes, Guid documentId, string documentName, int pageNumber)
    {
        return new Image
        {
            Id = Guid.NewGuid(),
            DocumentGroupId = documentId,
            DocumentName = documentName,
            Bytes = Convert.ToBase64String(imageBytes),
            PageNumber = pageNumber,
        };
    }
}