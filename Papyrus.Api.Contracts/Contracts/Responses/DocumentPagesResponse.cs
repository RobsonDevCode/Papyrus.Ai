namespace Papyrus.Api.Contracts.Contracts.Responses;

public record DocumentPagesResponse
{
    public IEnumerable<DocumentPageResponse> Pages { get; init; }
    
    public int TotalPages { get; init; }
}