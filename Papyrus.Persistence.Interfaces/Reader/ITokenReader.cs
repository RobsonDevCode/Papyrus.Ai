using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface ITokenReader
{
    Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken);
}