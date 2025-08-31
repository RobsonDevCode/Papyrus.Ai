using Papyrus.Domain.Models;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IImageDomainMapper
{
    ImageModel? MapToDomain(Image? image);
}