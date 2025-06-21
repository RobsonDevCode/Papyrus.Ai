using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Reader;
using Papyrus.Perstistance.Interfaces.Writer;
using UglyToad.PdfPig;
using PdfExtensions = Papyrus.Domain.Extensions.PdfExtensions;

namespace Papyrus.Domain.Services;

public sealed class DocumentWriterService : IDocumentWriterService
{
    private readonly IDocumentWriter _documentWriter;
    private readonly IDocumentReader _documentReader;
    private readonly IMapper _mapper;
    private readonly ILogger<DocumentWriterService> _logger;

    public DocumentWriterService(IDocumentWriter documentWriter,
        IDocumentReader documentReader,
        IMapper mapper,
        ILogger<DocumentWriterService> logger)
    {
        _documentWriter = documentWriter;
        _documentReader = documentReader;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task StoreDocumentAsync(DocumentModel document, CancellationToken cancellationToken)
    {
        if (await _documentReader.ExistsAsync(document.Name, cancellationToken))
        {
            throw new BadHttpRequestException($"Document with name {document.Name} already exists.");
        }
        
        using var pdfDoc = PdfDocument.Open(document.PdfStream);
        var totalPages = pdfDoc.NumberOfPages;
        if (totalPages == 0)
        {
            throw new InvalidOperationException("Cannot store empty document.");
        }

        var groupId = Guid.NewGuid();
        document.Name = document.Name.Replace(".pdf", "");
        

        for (var i = 0; i < totalPages; i++)
        {
            var pdfPageNumber = i + 1;
            var page = pdfDoc.GetPage(pdfPageNumber);
            
            if (string.IsNullOrWhiteSpace(page.Text) && !page.GetImages().Any())
            {
                _logger.LogWarning("Cannot extract page {pageNum} as its null skipping it.", pdfPageNumber);
                continue;
            }
            
            if (string.IsNullOrWhiteSpace(page.Text) && page.GetImages().Any())
            {
                var imageOnlyPage = new Page
                {
                    DocumentGroupId = groupId,
                    DocumentName = document.Name,
                    Content = null,
                    PageNumber = pdfPageNumber, // Pages are 1-indexed
                    Type = "pdf",
                    IsImageOnly = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PageImage = PdfExtensions.ConvertPdfPageToImage(pdfPageNumber, document.PdfStream),
                };
                
                await _documentWriter.WriteDocumentAsync(imageOnlyPage, cancellationToken);
                continue;
            }
            
            var mappedPage = new Page
            {
                DocumentGroupId = groupId,
                DocumentName = document.Name,
                Content = string.Join(" ", page.GetWords()),
                PageNumber = pdfPageNumber, // Pages are 1-indexed
                IsImageOnly = string.IsNullOrWhiteSpace(page.Text) && page.GetImages().Any(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PageImage = PdfExtensions.ConvertPdfPageToImage(pdfPageNumber, document.PdfStream),
                Type = "pdf"
            };

            await _documentWriter.WriteDocumentAsync(mappedPage, cancellationToken);
        }
    }
}