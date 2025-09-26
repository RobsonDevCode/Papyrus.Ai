using System.Text;
using Papyrus.Domain.Models.Voices;

namespace Papyrus.Domain.Extensions;

public static class ElevenLabsQueryExtensions
{
    public static string ToQueryString(this string url, VoiceSearchModel voiceSearchModel)
    {
        var queryBuilder = new StringBuilder();
        var hasParams = false;

        // Build the search parameter first
        if (!string.IsNullOrWhiteSpace(voiceSearchModel.SearchTerm) || 
            !string.IsNullOrWhiteSpace(voiceSearchModel.UseCase) || 
            !string.IsNullOrWhiteSpace(voiceSearchModel.Accent))
        {
            var searchParts = new List<string>();
        
            if (!string.IsNullOrWhiteSpace(voiceSearchModel.SearchTerm))
                searchParts.Add(voiceSearchModel.SearchTerm.Trim());
            
            if (!string.IsNullOrWhiteSpace(voiceSearchModel.UseCase))
                searchParts.Add(voiceSearchModel.UseCase.Trim());
            
            if (!string.IsNullOrWhiteSpace(voiceSearchModel.Accent))
                searchParts.Add(voiceSearchModel.Accent.Trim());

            if (searchParts.Any())
            {
                var searchValue = string.Join(" ,", searchParts);
                queryBuilder.Append($"search=\"{Uri.EscapeDataString(searchValue)}\"");
                hasParams = true;
            }
        }

        if (voiceSearchModel.Pagination.Page > 0)
        {
            if (hasParams) queryBuilder.Append('&');
            queryBuilder.Append($"page_size={voiceSearchModel.Pagination.Page}");
            hasParams = true;
        }

        return hasParams ? $"{url}?{queryBuilder}" : url;
    }
   
}