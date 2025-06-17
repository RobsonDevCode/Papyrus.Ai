using Papyrus.Domain.Models;
using Papyrus.Perstistance.Interfaces.Contracts;

namespace Papyrus.Domain.Extensions;

public static class PdfContentExtensions
{
    public static (IEnumerable<string?> OrderedContent, List<ImageModel> Images) ExtractContentFromPage(this UglyToad.PdfPig.Content.Page page, int pageNumber)
    {
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
                PageReference = pageNumber
            });

            imageIndex++;
        }

        // Sort and build single content string
        return (allContent
            .OrderByDescending(coords => coords.Y)
            .ThenBy(coords => coords.X)
            .Select(coords => coords.Content.ToString()), images);
    }

    public static List<Image> ExtractImages(this UglyToad.PdfPig.Content.Page page, int pageNumber)
    {
        return page.GetImages().Select(image => new Image
            { 
                Id = Guid.NewGuid(),
                Bytes = Convert.ToBase64String(image.RawBytes.ToArray()), 
                Width = image.WidthInSamples,
                Height = image.HeightInSamples, 
                PageNumber = pageNumber 
            }).ToList();
    }
}