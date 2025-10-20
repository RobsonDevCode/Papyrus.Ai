using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Models.Filters;
using Papyrus.Domain.Models.Pagination;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;

namespace Papyrus.Domain.Services;

public sealed class DocumentReaderService : IDocumentReaderService
{
    private readonly IDocumentReader _documentReader;
    private readonly IMapper _mapper;

    public DocumentReaderService(IDocumentReader documentReader, IMapper mapper)
    {
        _documentReader = documentReader;
        _mapper = mapper;
    }

    public async Task<PagedResponseModel<DocumentPreviewModel>> GetDocuments(Guid userId, string? searchTerm, PaginationRequestModel pagination, CancellationToken cancellationToken)
    {
        var response = await _documentReader.GetPagedDocumentsAsync(userId, searchTerm, new PaginationOptions
        {
            Page = pagination.Page,
            Size = pagination.Size,
        }, cancellationToken);

        if (response.Data.Count is 0)
        {
            return new PagedResponseModel<DocumentPreviewModel>
            {
                Items = [],
                Pagination = new PaginationModel
                {
                    Page = pagination.Page,
                    Size = pagination.Size,
                    TotalPages = response.TotalPages,
                }
            };
        }

        return _mapper.MapToDomain(response, pagination.Page, pagination.Size);
    }
}