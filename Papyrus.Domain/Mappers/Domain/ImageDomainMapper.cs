using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public partial class Mapper
{
    public ImageModel MapToDomain(Image image)
    {
        return new ImageModel
        {
            Bytes = image.Bytes,
            Width = image.Width,
            Height = image.Height,
            PageReference = image.PageNumber
        };
    }
}