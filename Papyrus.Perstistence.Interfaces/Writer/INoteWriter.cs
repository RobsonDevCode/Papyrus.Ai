using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Writer;

public interface INoteWriter
{
    Task SaveNoteAsync(Note note, CancellationToken cancellationToken);
    Task<Note> UpdateNoteAsync(Guid noteId, string editedNote, CancellationToken cancellationToken);
}