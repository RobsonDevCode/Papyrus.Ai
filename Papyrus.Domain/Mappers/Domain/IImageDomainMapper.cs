using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers;

public interface IImageDomainMapper
{
    ImageModel MapToDomain(Image image);
}