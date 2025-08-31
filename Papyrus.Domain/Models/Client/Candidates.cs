using Papyrus.Domain.Clients;

namespace Papyrus.Domain.Models.Client;

public record Candidates
{
    public required Content Content { get; init; }
}