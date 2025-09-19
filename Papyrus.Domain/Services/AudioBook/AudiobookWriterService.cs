using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NAudio.Lame;
using NAudio.Wave;
using Papyrus.Domain.Clients;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudiobookWriterService : IAudiobookWriterService
{
    private readonly IPageReader _pageReader;
    private readonly IAudioClient _audioClient;
    private readonly IAudioWriter _audioWriter;
    private readonly ILogger<AudiobookWriterService> _logger;
    private readonly string _papyrusApiUrl;
    
    public AudiobookWriterService(IPageReader pageReader,
        IAudioClient audioClient,
        IAudioWriter audioWriter,
        ILogger<AudiobookWriterService> logger,
        IConfiguration configuration)
    {
        _pageReader = pageReader;
        _audioClient = audioClient;
        _audioWriter = audioWriter;
        _logger = logger;
        _papyrusApiUrl = configuration.GetValue<string>("PapyrusApiUrl") ?? throw new NullReferenceException("PapyrusApiUrl cannot be null when creating an audio file");
    }

    public async Task<byte[]> CreateAsync(Guid documentGroupId, string voiceId, int[] pageNumbers, CancellationToken cancellationToken)
    {
        var outputStream = new MemoryStream();
        
        await using var mp3Writer = new LameMP3FileWriter(outputStream, new WaveFormat(42000, 2), LAMEPreset.STANDARD);
        var (pages, _) = await _pageReader.GetPages(documentGroupId, pageNumbers, cancellationToken);
        throw new NotImplementedException();
    }


    private async Task<byte[]> CreateEntireAudioFileAsync(Guid documentGroupId, string voiceId,
        MemoryStream outputStream, LameMP3FileWriter mp3Writer ,CancellationToken cancellationToken)
    {
        await foreach (var page in _pageReader.GetByGroupIdAsync(documentGroupId, cancellationToken))
        {
            if (page is null || string.IsNullOrEmpty(page.Content))
            {
                continue;
            }

            await using var audioStream =
                await _audioClient.CreateAudioAsync(voiceId, page.Content, cancellationToken);
            await using var mp3Reader = new Mp3FileReader(audioStream);

            await mp3Reader.CopyToAsync(mp3Writer, cancellationToken);
        }

        outputStream.Position = 0;
        using var saveStream = new MemoryStream(outputStream.ToArray());

        var s3Key = $"mp3s/{documentGroupId}/{voiceId}";
        await _audioWriter.SaveAsync(s3Key, saveStream, cancellationToken);

        return outputStream.ToArray();
    }
}