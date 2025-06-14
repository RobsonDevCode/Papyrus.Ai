using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;
public partial class Mapper 
{
    public Image Map(ImageModel image)
    {
        

        return new Image
        {
            Id = Guid.NewGuid(),
            Bytes = image.Bytes,
            Width = image.Width,
            Height = image.Height,
            PageNumber = image.PageReference,
        };
    }

    public List<Image> Map(List<ImageModel> images)
    {
        return images.Select(Map).ToList();
    }
}