
using Papyrus.Domain.Models.Filters;

namespace Papyrus.Domain.Models.Voices;

public record VoiceSearchModel
{
    public required PaginationRequestModel Pagination { get; init; }
    public string? SearchTerm { get; init; }
    public string? Accent { get; init; }
    public string? UseCase { get; init; }
    
    public string? Gender { get; init; }
}