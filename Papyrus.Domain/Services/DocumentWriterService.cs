using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Writer;

namespace Papyrus.Domain.Services;

public sealed class DocumentWriterService : IDocumentWriterService
{
    private readonly IDocumentWriter _documentWriter;
    private readonly ILogger<DocumentWriterService> _logger;

    public DocumentWriterService(IDocumentWriter documentWriter, ILogger<DocumentWriterService> logger)
    {
        _documentWriter = documentWriter;
        _logger = logger;
    }

    public async Task StoreDocumentAsync(DocumentModel documentModel, CancellationToken cancellationToken)
    {
        using var pdfReader = new PdfReader(documentModel.PdfStream);
        using var pdfDoc = new PdfDocument(pdfReader);

        var totalPages = pdfDoc.GetNumberOfPages();
        if (totalPages == 0)
        {
            throw new InvalidOperationException("Cannot store empty document.");
        }

        var groupId = Guid.NewGuid();
        var pages = new List<string>();

        for (int i = 1; i <= totalPages; i++)
        {
            var page = pdfDoc.GetPage(i);
            var extractedText = PdfTextExtractor.GetTextFromPage(page);
            pages.Add(extractedText);
        }

        await Parallel.ForAsync(0, pages.Count, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = cancellationToken
        }, async (i, ctx) =>
        {
            var extractedPage = pages[i];
            if (string.IsNullOrEmpty(extractedPage))
            {
                _logger.LogWarning("Cannot extract page {pageNum} as its null skipping it.", i + 1);
                return;
            }

            var mappedToDomain = new Page
            {
                DocumentGroupId = groupId,
                DocumentName = documentModel.Name,
                Content = extractedPage,
                PageNumber = i + 1, // Pages are 1-indexed
                DocumentType = "Pdf",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _documentWriter.WriteDocumentAsync(mappedToDomain, ctx);
        });
    }
}