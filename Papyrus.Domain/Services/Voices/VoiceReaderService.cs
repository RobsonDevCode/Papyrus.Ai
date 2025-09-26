using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Domain.Models.Voices;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.Voices;

public sealed class VoiceReaderService : IVoiceReaderService
{
    private readonly IVoiceReader _voiceReader;
    private readonly IMapper _mapper;

    public VoiceReaderService(IVoiceReader voiceReader, IMapper mapper)
    {
        _voiceReader = voiceReader;
        _mapper = mapper;
    }

    public async Task<PagedResponseModel<VoiceModel>> GetVoices(VoiceSearchModel filter,
        CancellationToken cancellationToken)
    {
        var mappedFilter = _mapper.MapToPersistence(filter);
        var response = await _voiceReader.GetVoicesAsync(mappedFilter, cancellationToken);

        return new PagedResponseModel<VoiceModel>
        {
            Items = _mapper.MapToDomain(response.Voices).ToArray(),
            Pagination = new PaginationModel
            {
                Page = filter.Pagination.Page,
                Size = filter.Pagination.Size,
                TotalPages = response.TotalCount
            }
        };
    }
}