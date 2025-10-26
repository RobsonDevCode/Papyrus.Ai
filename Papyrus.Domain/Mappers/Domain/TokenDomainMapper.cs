using Papyrus.Domain.Models.Authentication;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public RefreshTokenModel MapToDomain(RefreshToken refreshToken)
    {
        return new RefreshTokenModel
        {
            Token = refreshToken.Token,
            UserId = refreshToken.UserId,
            ExpiresAt = refreshToken.ExpiresAt,
            CreatedAt = refreshToken.CreatedAt,
            IsRevoked = refreshToken.IsRevoked,
        };
    }
}