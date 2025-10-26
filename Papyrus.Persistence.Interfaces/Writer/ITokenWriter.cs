using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface ITokenWriter
{
    Task SaveAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    
    Task DeleteAsync(string refreshToken, CancellationToken cancellationToken);
}