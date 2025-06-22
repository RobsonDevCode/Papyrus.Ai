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

        if (totalPages <= 15)
        {
            //small enough we can afford to load it all in memory 
            await StoreSmallPdf(pdfDoc, document, groupId, totalPages, cancellationToken);
        }

        var textOnlyPagesToSave = new List<Page>();
        const int batchSize = 25;
        for (var i = 0; i < totalPages; i++)
        {
            var pdfPageNumber = i + 1;
            var page = pdfDoc.GetPage(pdfPageNumber);
            
            if (string.IsNullOrWhiteSpace(page.Text) && !page.GetImages().Any())
            {
                _logger.LogWarning("Cannot extract page {pageNum} as its null skipping it.", pdfPageNumber);
                continue;
            }
            
            var imageCount = page.GetImages().Count();
            var orderedContent = page.ExtractContentFromPage();
            
            var mappedPage = new Page
            {
                DocumentGroupId = groupId,
                DocumentName = document.Name,
                Content = string.Join(" ", orderedContent),
                PageNumber = pdfPageNumber,
                ImageCount = imageCount,
                IsImageOnly = string.IsNullOrWhiteSpace(page.Text) && page.GetImages().Any(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = "pdf"
            };
            
            //If pdf page contains image we want to dispose as quick as possible because base64 encoding strings are expensive to store in memory.
            if (imageCount > 0)
            {
                mappedPage.PageImage = PdfExtensions.ConvertPdfPageToImage(pdfPageNumber, document.PdfStream);
                await _documentWriter.InsertAsync(mappedPage, cancellationToken);
                continue;
            }

            textOnlyPagesToSave.Add(mappedPage);
            if (textOnlyPagesToSave.Count >= batchSize || i == totalPages - 1)
            {
                await _documentWriter.InsertManyAsync(textOnlyPagesToSave, cancellationToken);
                textOnlyPagesToSave.Clear();
            }
        }
    }


    private async Task StoreSmallPdf(PdfDocument pdfDoc, DocumentModel document, Guid groupId, int totalPages, CancellationToken cancellationToken)
    {
        var pagesToStore = new List<Page>();
        for (var i = 0; i < totalPages; i++)
        {
            var pdfPageNumber = i + 1;
            var page = pdfDoc.GetPage(pdfPageNumber);
            
            if (string.IsNullOrWhiteSpace(page.Text) && !page.GetImages().Any())
            {
                _logger.LogWarning("Cannot extract page {pageNum} as its null skipping it.", pdfPageNumber);
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
                Type = "pdf"
            };
            
            if (page.GetImages().Any())
            {
                mappedPage.PageImage = PdfExtensions.ConvertPdfPageToImage(pdfPageNumber, document.PdfStream);
            }
            pagesToStore.Add(mappedPage);
        }

        await Parallel.ForAsync(0, pagesToStore.Count, new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = cancellationToken
            },
            async (i, ctx) =>
            {
                await _documentWriter.InsertAsync(pagesToStore[i], ctx);
            });
    }
}