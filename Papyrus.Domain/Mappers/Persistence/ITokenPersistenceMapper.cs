using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface ITokenPersistenceMapper
{
    RefreshToken MapToPersistence(Guid userId, string refreshToken);
}