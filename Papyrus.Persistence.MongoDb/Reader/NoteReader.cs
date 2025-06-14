using MongoDB.Driver;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class NoteReader : INoteReader
{
    private readonly IMongoCollection<Note> _noteCollection;
    
    public NoteReader(IMongoBookDbConnector connector)
    {
        _noteCollection = connector.GetCollection<Note>(nameof(Note));
    }
    
    public async Task<Note?> GetNoteAsync(Guid id, CancellationToken cancellationToken)
    {  
        return await _noteCollection.Find(n => n.Id == id).SingleOrDefaultAsync(cancellationToken);
    }
}