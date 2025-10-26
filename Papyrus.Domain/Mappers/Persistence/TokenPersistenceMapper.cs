using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper
{
    public RefreshToken MapToPersistence(Guid userId, string refreshToken)
    {
        return new RefreshToken
        {
            Token = refreshToken,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };
    }
}