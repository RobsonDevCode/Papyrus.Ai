using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface IUserReader
{
    Task<bool> ExistsAsync(string? username, string? email, CancellationToken cancellationToken);
    
    Task<User?> GetAsync(string? username, string? email, CancellationToken cancellationToken);
}