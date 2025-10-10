using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IUserWriter
{
    Task InsertAsync(User user, CancellationToken cancellationToken);
}