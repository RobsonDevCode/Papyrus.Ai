using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.Documents;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Persistance.Interfaces.Contracts;
using Papyrus.Persistance.Interfaces.Reader;
using Papyrus.Persistance.Interfaces.Writer;
using Papyrus.Perstistance.Interfaces.Writer;
using UglyToad.PdfPig;
using PdfExtensions = Papyrus.Domain.Extensions.PdfExtensions;

namespace Papyrus.Domain.Services;

public sealed class DocumentWriterService : IDocumentWriterService
{
    private readonly IPageWriter _pageWriter;
    private readonly IPageReader _pageReader;
    private readonly IDocumentWriter _documentWriter;
    private readonly IPdfWriterService _pdfWriterService;
    private readonly IImageWriter _imageWriter;
    private readonly IImageInfoWriterService _imageInfoWriterService;
    private readonly IMapper _mapper;
    private readonly ILogger<DocumentWriterService> _logger;
    private readonly string _papyrusApiUrl;

    public DocumentWriterService(IPageWriter pageWriter,
        IPageReader pageReader,
        IDocumentWriter documentWriter,
        IPdfWriterService pdfWriterService,
        IImageWriter imageWriter,
        IImageInfoWriterService imageInfoWriterService,
        IMapper mapper,
        IConfiguration configuration,
        ILogger<DocumentWriterService> logger)
    {
        _pageWriter = pageWriter;
        _pageReader = pageReader;
        _documentWriter = documentWriter;
        _pdfWriterService = pdfWriterService;
        _imageWriter = imageWriter;
        _imageInfoWriterService = imageInfoWriterService;
        _mapper = mapper;
        _papyrusApiUrl = configuration.GetValue<string>("PapyrusApiUrl")
                         ?? throw new NullReferenceException("PapyrusApiUrl cannot be null when saving document");
        _logger = logger;
    }

    public async Task StoreDocumentAsync(DocumentModel document, CancellationToken cancellationToken)
    {
        if (await _pageReader.ExistsAsync(document.Name, cancellationToken))
        {
            throw new BadHttpRequestException($"Document with name {document.Name} already exists.");
        }

        var groupId = Guid.NewGuid();
        var s3PdfKey = $"pdfs/{DateTime.UtcNow:yyyy/MM/dd}/{groupId}_{document.Name}";
        using var savePdfStream = new MemoryStream();
        await document.PdfStream.CopyToAsync(savePdfStream, cancellationToken);
        savePdfStream.Position = 0;

        await _pdfWriterService.SaveAsync(s3PdfKey, savePdfStream, cancellationToken);
        document.PdfStream.Position = 0;

        using var pdfDoc = PdfDocument.Open(document.PdfStream);
        if (pdfDoc.NumberOfPages == 0)
        {
            throw new InvalidOperationException("Cannot store empty document.");
        }

        document.Name = document.Name.Replace(".pdf", "");

        await SavePdfPagesAsync(pdfDoc, document, groupId, s3PdfKey, cancellationToken);
    }

    private async Task SavePdfPagesAsync(PdfDocument pdfDoc, DocumentModel document,
        Guid groupId, string s3PdfKey, CancellationToken cancellationToken)
    {
        var textOnlyPagesToSave = new List<Page>();
        const int batchSize = 25;
        var totalPages = pdfDoc.NumberOfPages;
        
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
                S3Key = s3PdfKey,
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
                await using var imageStream = PdfExtensions.ConvertPdfPageToImage(pdfPageNumber, document.PdfStream);
                var s3Key = $"image/{groupId}/{pdfPageNumber}";

                await _imageWriter.SaveAsync(s3Key, imageStream, cancellationToken);

                var image = _mapper.MapToPersistence(groupId, document.Name, mappedPage.PageNumber, s3Key);
                mappedPage.ImageUrl = _papyrusApiUrl + "/image/" + image.Id;

                var tasks = new[]
                {
                    _imageInfoWriterService.InsertAsync(image, cancellationToken),
                    _pageWriter.InsertAsync(mappedPage, cancellationToken)
                };

                await Task.WhenAll(tasks);
                if (pdfPageNumber != 1)
                {
                    continue;
                }
            }

            if (pdfPageNumber == 1)
            {
                var saveDocumentInfo = _mapper.MapToPersistence(mappedPage, totalPages);
                await _documentWriter.InsertAsync(saveDocumentInfo, cancellationToken);
                continue;
            }

            textOnlyPagesToSave.Add(mappedPage);
            if (textOnlyPagesToSave.Count < batchSize && i != totalPages - 1) continue;
            
            await _pageWriter.InsertManyAsync(textOnlyPagesToSave, cancellationToken);
            textOnlyPagesToSave.Clear();
        }
    }
}