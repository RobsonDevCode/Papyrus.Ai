using Papyrus.Domain.Clients;

namespace Papyrus.Domain.Extensions;

public static class LlmExtensions
{
    public static string ExtractResponse(this LlmResponseModel llmResponseModel)
    {
        var candidate = llmResponseModel.Candidates.SingleOrDefault();
        if (candidate is null)
        {
            throw new NullReferenceException("Candidate is null when trying to extract the response from the llm.");
        }
        
        var part = candidate.Content.Parts.SingleOrDefault();
        if (part is null)
        {
            throw new NullReferenceException("Part is null when trying to extract the response from the llm.");
        }
        
        return part.Text ?? throw new NullReferenceException("Text is null when trying to extract the response from the llm.");
    }
}