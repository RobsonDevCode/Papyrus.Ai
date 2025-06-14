using MongoDB.Driver;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class NoteWriter : INoteWriter
{
    private readonly IMongoCollection<Note> _notesCollection;

    public NoteWriter(IMongoBookDbConnector connector)
    {
        _notesCollection = connector.GetCollection<Note>(nameof(Note));    
    }        
    
    public async Task SaveNoteAsync(Note note, CancellationToken cancellationToken)
    {
        await _notesCollection.InsertOneAsync(note, null, cancellationToken);
    }
}