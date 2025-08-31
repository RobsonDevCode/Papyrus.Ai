using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Persistance.Interfaces.Writer;

public interface INoteWriter
{
    Task SaveNoteAsync(Note note, CancellationToken cancellationToken);
    Task<Note?> UpdateNoteAsync(Guid noteId, string editedNote, CancellationToken cancellationToken);
}