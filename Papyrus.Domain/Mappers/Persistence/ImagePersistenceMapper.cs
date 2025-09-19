using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;
public partial class Mapper 
{
    public Image MapToPersistence(Guid documentId, string documentName, int pageNumber, string s3Key)
    {
        return new Image
        {
            Id = Guid.NewGuid(),
            DocumentGroupId = documentId,
            DocumentName = documentName,
            PageNumber = pageNumber,
            S3Key = s3Key,
        };
    }
}