using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Mappers;

public partial class Mapper 
{
    public User MapToPersistence(string username, string email, string passwordHash, string? name = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
}