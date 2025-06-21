using SkiaSharp;
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
}