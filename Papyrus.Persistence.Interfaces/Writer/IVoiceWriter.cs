using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface IVoiceWriter
{
    Task InsertVoicesAsync(IEnumerable<Voice> voices);
}