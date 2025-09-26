using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Contracts.Filters;

namespace Papyrus.Perstistance.Interfaces.Reader;

public interface IVoiceReader
{
    Task<(IEnumerable<Voice> Voices, int TotalCount)> GetVoicesAsync(VoiceSearch filter, CancellationToken cancellationToken);
}