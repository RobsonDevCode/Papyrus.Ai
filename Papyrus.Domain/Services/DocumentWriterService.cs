using System.Text;
using Microsoft.Extensions.Logging;
using Papyrus.Domain.Extensions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models;
using Papyrus.Domain.Services.Interfaces;
using Papyrus.Perstistance.Interfaces.Contracts;
using Papyrus.Perstistance.Interfaces.Writer;
using UglyToad.PdfPig;

namespace Papyrus.Domain.Services;

public sealed class DocumentWriterService : IDocumentWriterService
{
    private readonly IDocumentWriter _documentWriter;
    private readonly IMapper _mapper;
    private readonly ILogger<DocumentWriterService> _logger;

    public DocumentWriterService(IDocumentWriter documentWriter,
        IMapper mapper,
        ILogger<DocumentWriterService> logger)
    {
        _documentWriter = documentWriter;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task StoreDocumentAsync(DocumentModel documentModel, CancellationToken cancellationToken)
    {
        using var pdfDoc = PdfDocument.Open(documentModel.PdfStream);

        var totalPages = pdfDoc.NumberOfPages;
        if (totalPages == 0)
        {
            throw new InvalidOperationException("Cannot store empty document.");
        }

        var groupId = Guid.NewGuid();

        var pagesToSave = new List<Page>(totalPages);

        for (var i = 0; i < totalPages; i++)
        {
            var pdfPageNumber = i + 1;
            var page = pdfDoc.GetPage(pdfPageNumber);
            if (string.IsNullOrWhiteSpace(page.Text))
            {
                _logger.LogWarning("Cannot extract page {pageNum} as its null skipping it.", pdfPageNumber);
                continue;
            }

            // Get all content with positions
            var allContent = new List<(double Y, double X, object Content)>();

            // Add words
            foreach (var word in page.GetWords())
            {
                allContent.Add((
                    Y: word.BoundingBox.Bottom,
                    X: word.BoundingBox.Left,
                    Content: word.Text
                ));
            }

            // Add images and text placeholders
            var images = new List<ImageModel>();
            var imageIndex = 0;
            foreach (var image in page.GetImages())
            {
                allContent.Add((
                    Y: image.Bounds.Bottom,
                    X: image.Bounds.Left,
                    Content: $"$$IMAGE_{imageIndex}$$" //will be used to put the image in the correct position
                ));

                images.Add(new ImageModel
                {
                    Bytes = Convert.ToBase64String(image.RawBytes.ToArray()),
                    Width = image.WidthInSamples,
                    Height = image.HeightInSamples,
                    PageReference = pdfPageNumber
                });
                
                imageIndex++;
            }

            // Sort and build single content string
            var orderedContent = allContent
                .OrderByDescending(coords => coords.Y)
                .ThenBy(coords => coords.X)
                .Select(coords => coords.Content.ToString())
                .ToList();

            var mappedPage = new Page
            {
                DocumentGroupId = groupId,
                DocumentName = documentModel.Name,
                Content = string.Join(" ", orderedContent),
                PageNumber = pdfPageNumber, // Pages are 1-indexed
                DocumentType = "Pdf",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = _mapper.Map(images)
            };

            pagesToSave.Add(mappedPage);

            // Force cleanup of Letter objects after 5 pages
            if (i % 20 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        
        await Parallel.ForAsync(0, pagesToSave.Count, new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = cancellationToken
        }, async (i, ctx) => { await _documentWriter.WriteDocumentAsync(pagesToSave[i], ctx); });
    }
}