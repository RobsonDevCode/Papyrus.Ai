using Papyrus.Domain.Models;
using UglyToad.PdfPig.Content;

namespace Papyrus.Domain.Extensions;

public static class ImageExtensions
{
    public static List<ImageModel> GetImagesFromPage(this Page page, int pageNumber)
    {
        return page.GetImages().Select(image => new ImageModel
        {
            Bytes = Convert.ToBase64String(image.RawBytes.ToArray()),
            Width = image.WidthInSamples,
            Height = image.HeightInSamples,
            PageReference = pageNumber
        }).ToList();
    }
    
}