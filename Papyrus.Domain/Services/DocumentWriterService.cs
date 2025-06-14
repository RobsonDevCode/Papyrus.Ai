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
        using var pdfDoc= PdfDocument.Open(documentModel.PdfStream);
        
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
            
            var mappedPage = new Page
            {
                DocumentGroupId = groupId,
                DocumentName = documentModel.Name,
                Content = string.Join(" ", page.GetWords()),
                PageNumber = pdfPageNumber, // Pages are 1-indexed
                DocumentType = "Pdf",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = _mapper.Map(page.GetImagesFromPage(pdfPageNumber))
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
        }, async (i, ctx) =>
        {
            await _documentWriter.WriteDocumentAsync(pagesToSave[i], ctx);
        });
    }
}