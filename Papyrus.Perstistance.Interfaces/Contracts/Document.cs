namespace Papyrus.Perstistance.Interfaces.Contracts;

public record Document
{
    public required Guid DocumentId { get; init; }

    public required string DocumentTitle { get; init; }

    public List<Page> Pages { get; init; }
}