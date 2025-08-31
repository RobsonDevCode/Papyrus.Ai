using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Reader;

public interface IPromptHistoryReader
{
    Task<List<Prompt>> GetHistory(Guid noteId, CancellationToken cancellationToken);
}