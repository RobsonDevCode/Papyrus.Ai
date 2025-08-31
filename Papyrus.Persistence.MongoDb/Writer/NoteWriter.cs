using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Mongo;
using Papyrus.Persistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class NoteWriter : INoteWriter
{
    private readonly IMongoCollection<Note> _notesCollection;

    public NoteWriter(IMongoBookDbConnector connector)
    {
        _notesCollection = connector.GetCollection<Note>(DatabaseConstants.NotesCollectionName);
    }

    public async Task SaveNoteAsync(Note note, CancellationToken cancellationToken)
    {
        await _notesCollection.InsertOneAsync(note, null, cancellationToken);
    }

    public async Task<Note?> UpdateNoteAsync(Guid noteId, string editedNote,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Note>.Filter.Eq(n => n.Id, noteId);
        var update = Builders<Note>.Update
            .Set(x => x.Text, editedNote)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<Note>
        {
            ReturnDocument = ReturnDocument.After,
        };
            
        var updatedNote = await _notesCollection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
        
        return updatedNote;
    }
}