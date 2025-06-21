using Microsoft.Extensions.Caching.Memory;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Domain.Services.Interfaces.Notes;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.Notes;

public sealed class NoteReaderService : INoteReaderService
{
    private readonly INoteReader _noteReader;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public NoteReaderService(INoteReader noteReader, IMemoryCache memoryCache, IMapper mapper)
    {
        _noteReader = noteReader;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async ValueTask<NoteModel?> GetNoteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(id, async entry =>
        {
            entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            var response = await _noteReader.GetNoteAsync(id, cancellationToken);

            return response == null ? null : _mapper.MapToDomain(response);
        });
    }

    public async Task<PagedResponseModel<NoteModel>?> GetNotesAsync(Guid id, PaginationRequestModel options,
        CancellationToken cancellationToken)
    {
        var response = await _noteReader.GetPagedNotesAsync(id, new PaginationOptions
        {
            Page = options.Page,
            Size = options.Size,
        }, cancellationToken);

        if (response.Data.Count is 0)
        {
            return new PagedResponseModel<NoteModel>
            {
                Items = [],
                Pagination = new PaginationModel
                {
                    Page = options.Page,
                    Size = options.Size,
                    TotalPages = response.TotalPages
                }
            };
        }
        
        return _mapper.Map(response, options.Page, options.Size);
    }

    public async ValueTask<List<NoteModel>?> GetNotesOnPageAsync(Guid documentGroupId, int pdfPage,
        CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync($"{documentGroupId}-{pdfPage}", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
            
            var response = await _noteReader.GetNotesOnPageAsync(documentGroupId, pdfPage, cancellationToken);
            return _mapper.Map(response);
        });
    }
}