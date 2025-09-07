using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NAudio.Lame;
using NAudio.Wave;
using Papyrus.Domain.Models.Client.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudiobookWriterService : IAudiobookWriterService
{
    private readonly IDocumentReader _documentReader;
    private readonly IAudioClient _audioClient;
    private readonly IAudioWriter _audioWriter;
    private readonly ILogger<AudiobookWriterService> _logger;

    public AudiobookWriterService(IDocumentReader documentReader,
        IAudioClient audioClient,
        IAudioWriter audioWriter,
        ILogger<AudiobookWriterService> logger)
    {
        _documentReader = documentReader;
        _audioClient = audioClient;
        _audioWriter = audioWriter;
        _logger = logger;
    }

    public async Task<byte[]> CreateAsync(Guid documentGroupId, string voiceId, CancellationToken cancellationToken)
    {
        var outputStream = new MemoryStream();
        
        await using var mp3Writer = new LameMP3FileWriter(outputStream, new WaveFormat(42000, 2), LAMEPreset.STANDARD);
        await foreach (var page in _documentReader.GetByGroupIdAsync(documentGroupId, cancellationToken))
        {
            if (page is null || string.IsNullOrEmpty(page.Content))
            {
                continue;
            }

            await using var audioStream = await _audioClient.CreateAudioAsync(voiceId, page.Content, cancellationToken);
            await using var mp3Reader = new Mp3FileReader(audioStream);

            await mp3Reader.CopyToAsync(mp3Writer, cancellationToken);
        }

        outputStream.Position = 0;
        var saveStream = new MemoryStream();
        await mp3Writer.CopyToAsync(saveStream, cancellationToken);

        var s3Key = $"mp3s/{documentGroupId}/{voiceId}";
        await _audioWriter.SaveAsync(s3Key, saveStream, cancellationToken);

        return outputStream.ToArray();
    }
}