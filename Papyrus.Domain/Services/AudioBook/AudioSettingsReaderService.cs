using Microsoft.Extensions.Caching.Memory;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Audio;
using Papyrus.Domain.Services.Interfaces.AudioBook;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.AudioBook;

public sealed class AudioSettingsReaderService : IAudioSettingsReaderService
{
    private readonly IAudioSettingReader _audioSettingReader;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public AudioSettingsReaderService(IAudioSettingReader audioSettingReader, IMemoryCache cache, IMapper mapper)
    {
        _audioSettingReader = audioSettingReader;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<AudioSettingsModel?> GetAsync(CancellationToken cancellationToken)
    {
        return await _cache.GetOrCreateAsync("", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));

            var audioSettings = await _audioSettingReader.GetAsync(cancellationToken);
            return audioSettings == null ? null : _mapper.MapToDomain(audioSettings);
        });
    }

    public async Task<AudioSettingsModel?> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _cache.GetOrCreateAsync(userId, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            var audioSettings = await _audioSettingReader.GetAsync(userId, cancellationToken);
            return audioSettings == null ? null : _mapper.MapToDomain(audioSettings);
        });
    }
}