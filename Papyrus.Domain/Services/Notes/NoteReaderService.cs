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

    public async Task<PagedResponseModel<NoteModel>> GetNotesAsync(Guid documentId, PaginationRequestModel options,
        int? pdfPageNumber,
        CancellationToken cancellationToken)
    {
        var response = await _noteReader.GetPagedNotesAsync(documentId, new PaginationOptions
        {
            Page = options.Page,
            Size = options.Size,
            PdfPage = pdfPageNumber
        }, cancellationToken);

        return response.Data.Count is 0
            ? new PagedResponseModel<NoteModel>()
            : _mapper.Map(response, options.Page, options.Size);
    }
}