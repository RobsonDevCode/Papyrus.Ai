using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistence.MongoDb.Connectors;

namespace Papyrus.Persistence.MongoDb.Reader;

public sealed class DocumentReader : IDocumentReader
{
    private readonly IMongoCollection<DocumentPreview> _documentCollection;

    public DocumentReader(IMongoBookDbConnector connector)
    {
        _documentCollection = connector.GetCollection<DocumentPreview>(DatabaseConstants.DocumentsCollectionName);
    }

    public async Task<PagedResponse<DocumentPreview>> GetPagedDocumentsAsync(Guid userId, string? searchTerm,
        PaginationOptions pagination, CancellationToken cancellationToken)
    {
        List<DocumentPreview> documents;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            documents = await _documentCollection.Find(x => x.UserId == userId)
                .Skip((pagination.Page - 1) * pagination.Size)
                .Limit(pagination.Size)
                .ToListAsync(cancellationToken);
        }
        else
        {
            documents = await _documentCollection.Find(x => x.UserId == userId
                                                            && x.Name.Contains(searchTerm))
                .Skip((pagination.Page - 1) * pagination.Size)
                .Limit(pagination.Size)
                .ToListAsync(cancellationToken);
        }


        var totalCount = await _documentCollection.CountDocumentsAsync(x => true, cancellationToken: cancellationToken);


        return new PagedResponse<DocumentPreview>
        {
            Data = documents,
            TotalPages = (int)totalCount
        };
    }
}