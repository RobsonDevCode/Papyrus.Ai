using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class NoteReader : INoteReader
{
    private readonly IMongoCollection<Note> _noteCollection;

    public NoteReader(IMongoBookDbConnector connector)
    {
        _noteCollection = connector.GetCollection<Note>(DatabaseConstants.NotesCollectionName);
    }

    public async Task<Note?> GetNoteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _noteCollection.Find(n => n.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResponse<Note>> GetPagedNotesAsync(Guid id, PaginationOptions options,
        CancellationToken cancellationToken)
    {
        var notes = await _noteCollection.Find(n => n.Id == id)
            .Skip((options.Page - 1) * options.Size)
            .Limit(options.Size)
            .ToListAsync(cancellationToken);

        var totalCount = await _noteCollection.CountDocumentsAsync(x => x.DocumentGroupId == id,
            cancellationToken: cancellationToken);

        return new PagedResponse<Note>
        {
            Data = notes,
            TotalPages = (int)totalCount
        };
    }

    public async Task<List<Note>> GetNotesOnPageAsync(Guid documentGroupId, int pdfPage,
        CancellationToken cancellationToken)
    {
        return await _noteCollection.Find(n => n.DocumentGroupId == documentGroupId && n.RelatedPage == pdfPage)
            .ToListAsync(cancellationToken);
    }
}