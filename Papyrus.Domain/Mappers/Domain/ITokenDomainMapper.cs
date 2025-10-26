using Papyrus.Domain.Models.Authentication;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface ITokenDomainMapper
{
    RefreshTokenModel MapToDomain(RefreshToken refreshToken);
}