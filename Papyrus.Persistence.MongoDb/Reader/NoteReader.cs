﻿using MongoDB.Driver;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Mongo;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class NoteReader : INoteReader
{
    private readonly IMongoCollection<Note> _noteCollection;

    public NoteReader(IMongoBookDbConnector connector)
    {
        _noteCollection = connector.GetCollection<Note>("note");
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
            TotalPages = totalCount
        };
    }

    public async Task<List<Note>> GetNotesOnPageAsync(Guid documentGroupId, int pdfPage,
        CancellationToken cancellationToken)
    {
        return await _noteCollection.Find(n => n.DocumentGroupId == documentGroupId && n.RelatedPage == pdfPage)
            .ToListAsync(cancellationToken);
    }
}