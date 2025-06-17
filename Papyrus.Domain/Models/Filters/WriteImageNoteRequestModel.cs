namespace Papyrus.Domain.Models.Filters;

public record WriteImageNoteRequestModel
{
    public required Guid DocumentGroupId { get; init; }
    
    public required int ImageReference  { get; init; }
    
    public required int Page { get; init; }
}