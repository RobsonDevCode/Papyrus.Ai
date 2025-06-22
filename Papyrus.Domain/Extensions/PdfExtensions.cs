using SkiaSharp;
using UglyToad.PdfPig.Content;
using Conversion = PDFtoImage.Conversion;

namespace Papyrus.Domain.Extensions;

public static class PdfExtensions
{
    public static string ConvertPdfPageToImage(int pageNumber, Stream stream)
    {
        using var bitMap = Conversion.ToImage(stream,  pageNumber - 1, true);
        using var ms = new MemoryStream();
        
        bitMap.Encode(ms, SKEncodedImageFormat.Png, 100);
        return Convert.ToBase64String(ms.ToArray());
    }
    
    public static IEnumerable<string?> ExtractContentFromPage(this Page page) // possibly the most annoying method ive made
    {
        var allContent = new List<(double Y, double X, object Content)>();

        foreach (var word in page.GetWords())
        {
            allContent.Add((
                Y: word.BoundingBox.Bottom,
                X: word.BoundingBox.Left,
                Content: word.Text
            ));
        }

        // Add images and text placeholders
        var imageIndex = 0;
        foreach (var image in page.GetImages())
        {
            allContent.Add((
                Y: image.Bounds.Bottom,
                X: image.Bounds.Left,
                Content: $"$$IMAGE_{imageIndex}$$" //used to provide positional context for the fucking llm
            ));

            imageIndex++;
        }

        return allContent
            .OrderByDescending(coords => coords.Y)
            .ThenBy(coords => coords.X)
            .Select(coords => coords.Content.ToString());
    }
    
}