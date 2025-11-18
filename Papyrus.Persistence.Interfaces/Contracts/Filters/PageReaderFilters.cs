namespace Papyrus.Perstistance.Interfaces.Contracts.Filters;

public record PageReaderFilters
{
    public Guid? DocumentGroupId { get; init; }

    public Guid? Id { get; init; }

    public Guid? UserId { get; init; }
    
    public int? PageNumber { get; init; }
}