using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Papyrus.Ai.Configuration;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistance.Interfaces.Writer;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Persistence.MongoDb.Writer;

public sealed class DocumentTransactionalWriter : IDocumentTransactionalWriter
{
    private readonly IMongoClient _client;
    private readonly IOptions<MongoDbSettings> _options;
    private readonly IDocumentWriter _documentWriter;
    private readonly IPageWriter _pageWriter;
    private readonly IBookmarkWriter _bookmarkWriter;
    private readonly IAudioWriter _audioWriter;
    private readonly IAudioSettingsWriter _audioSettingsWriter;
    private readonly IAudioSettingReader _audioSettingReader;
    
    public DocumentTransactionalWriter(IOptions<MongoDbSettings> options,
        IDocumentWriter documentWriter,
        IPageWriter pageWriter,
        IBookmarkWriter bookmarkWriter,
        IAudioWriter audioWriter,
        IAudioSettingsWriter audioSettingsWriter,
        IAudioSettingReader audioSettingReader)
    {
        _options = options;        
        _client = new MongoClient(options.Value.ConnectionString);
        _documentWriter = documentWriter;
        _pageWriter = pageWriter;
        _bookmarkWriter = bookmarkWriter;
        _audioWriter = audioWriter;
        _audioSettingsWriter = audioSettingsWriter;
        _audioSettingReader = audioSettingReader;
    }
    
    
    public async Task DeleteDocumentTransaction(Guid userId, Guid documentGroupId, CancellationToken cancellationToken)
    {
        using var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();
        try
        {
            await _documentWriter.DeleteAsync(userId, documentGroupId, cancellationToken);
            await _pageWriter.DeleteAsync(userId, documentGroupId, cancellationToken);
            await _bookmarkWriter.DeleteAsync(userId, documentGroupId, cancellationToken);
            
            var audioSettings = await _audioSettingReader.GetByDocIdAsync(userId, documentGroupId, cancellationToken);
            if (audioSettings is not null)
            {
                var s3Key = $"{documentGroupId}-{audioSettings.VoiceId}";
                await _audioWriter.DeleteAsync(s3Key, cancellationToken);
                await _audioSettingsWriter.DeleteAsync(userId, documentGroupId, cancellationToken);
            }
        }
        catch
        {
             await session.AbortTransactionAsync(cancellationToken);
             throw;
        } 
    }
}