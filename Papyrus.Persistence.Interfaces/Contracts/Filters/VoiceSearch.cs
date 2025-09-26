using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Perstistance.Interfaces.Contracts.Filters;

public class VoiceSearch
{
    public required PaginationOptions Pagination { get; init; }
    public string? SearchTerm { get; init; }
    public string? Accent { get; init; }
    public string? UseCase { get; init; }
    public string? Gender { get; init; }
}