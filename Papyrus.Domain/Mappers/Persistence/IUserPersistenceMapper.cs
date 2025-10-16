using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Persistence;

public interface IUserPersistenceMapper
{
    User MapToPersistence(string username, string email, string passwordHash, string? name = null);
}