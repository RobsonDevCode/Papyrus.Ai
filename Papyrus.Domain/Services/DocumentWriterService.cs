using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
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
    private readonly IDocumentWriter _documentWriter;
    private readonly IDocumentReader _documentReader;
    private readonly IPdfWriterService _pdfWriterService;
    private readonly IImageWriterService _imageWriterService;
    private readonly IMapper _mapper;
    private readonly ILogger<DocumentWriterService> _logger;
    private readonly string _papyrusApiUrl;

    public DocumentWriterService(IDocumentWriter documentWriter,
        IDocumentReader documentReader,
        IPdfWriterService pdfWriterService,
        IImageWriterService imageWriterService,
        IMapper mapper,
        IConfiguration configuration,
        ILogger<DocumentWriterService> logger)
    {
        _documentWriter = documentWriter;
        _documentReader = documentReader;
        _pdfWriterService = pdfWriterService;
        _imageWriterService = imageWriterService;
        _mapper = mapper;
        _papyrusApiUrl = configuration.GetValue<string>("PapyrusApiUrl")
                         ?? throw new NullReferenceException("PapyrusApiUrl cannot be null when saving document");
        _logger = logger;
    }

    public async Task StoreDocumentAsync(DocumentModel document, CancellationToken cancellationToken)
    {
        if (await _documentReader.ExistsAsync(document.Name, cancellationToken))
        {
            throw new BadHttpRequestException($"Document with name {document.Name} already exists.");
        }

        var groupId = Guid.NewGuid();
        var s3Key = $"pdfs/{DateTime.UtcNow:yyyy/MM/dd}/{groupId}_{document.Name}";

        var savePdfStream = new MemoryStream();
        await document.PdfStream.CopyToAsync(savePdfStream, cancellationToken);
        savePdfStream.Position = 0;

        await _pdfWriterService.SaveAsync(s3Key, savePdfStream, cancellationToken);
        document.PdfStream.Position = 0;

        using var pdfDoc = PdfDocument.Open(document.PdfStream);
        var totalPages = pdfDoc.NumberOfPages;
        if (totalPages == 0)
        {
            throw new InvalidOperationException("Cannot store empty document.");
        }

        document.Name = document.Name.Replace(".pdf", "");

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
                S3Key = s3Key,
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
                var imageBytes = PdfExtensions.ConvertPdfPageToImage(pdfPageNumber, document.PdfStream);
                var image = _mapper.MapToPersistence(imageBytes, groupId, document.Name, mappedPage.PageNumber);
                mappedPage.ImageUrl = _papyrusApiUrl + "/image/" + image.Id;
                var tasks = new[]
                {
                    _imageWriterService.InsertAsync(image, cancellationToken),
                    _documentWriter.InsertAsync(mappedPage, cancellationToken)
                };

                await Task.WhenAll(tasks);
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
}