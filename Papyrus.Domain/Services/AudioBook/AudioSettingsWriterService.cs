using Papyrus.Domain.Clients;
using Papyrus.Domain.Exceptions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.AudioBook;

public class AudioSettingsWriterService : IAudioSettingsWriterService
{
    private readonly IAudioSettingsWriter _audioSettingsWriter;
    private readonly IAudioClient _audioClient;
    private readonly IMapper _mapper;

    public AudioSettingsWriterService(IAudioSettingsWriter audioSettingsWriter, IAudioClient audioClient,
        IMapper mapper)
    {
        _audioSettingsWriter = audioSettingsWriter;
        _audioClient = audioClient;
        _mapper = mapper;
    }

    public async Task Upsert(AudioSettingsModel settings, CancellationToken cancellationToken)
    {
        var voice = await _audioClient.GetVoiceAsync(settings.VoiceId, cancellationToken);
        if (voice is null)
        {
            throw new VoiceNotFoundException($"voice {settings.VoiceId} not found or doesnt exist");
        }

        var settingsToSave = _mapper.MapToPersistence(settings);
        
        await _audioSettingsWriter.UpsertAsync(settingsToSave, cancellationToken);
    }
}