using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface INoteWriter
{
    Task SaveNoteAsync(Note note, CancellationToken cancellationToken);
}