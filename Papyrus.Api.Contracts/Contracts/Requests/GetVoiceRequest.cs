
namespace Papyrus.Api.Contracts.Contracts.Requests;

public record GetVoiceRequest
{
    public string? SearchTerm {get; init; }
    public string? Accent { get; init; }
    public string? UseCase { get; init; }
    
    public string? Gender { get; init; }
}
