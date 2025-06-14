using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface IImagePersistenceMapper
{
    Image Map(ImageModel image);
    
    List<Image> Map(List<ImageModel> images);
}