using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public ImageModel? MapToDomain(Image? image)
    {
        if (image == null)
        {
            return null;
        }

        return new ImageModel
        {
            Id = image.Id,
            DocumentId = image.DocumentGroupId,
            Bytes = Convert.FromBase64String(image.Bytes),
            DocumentName = image.DocumentName
        };
    }
}