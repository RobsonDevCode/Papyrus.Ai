using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Writer;

public interface IPromptHistoryWriter
{
    Task InsertAsync(Prompt prompt, CancellationToken cancellationToken);
}