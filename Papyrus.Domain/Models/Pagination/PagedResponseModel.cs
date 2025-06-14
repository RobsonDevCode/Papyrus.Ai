namespace Papyrus.Domain.Models.Pagination;

public record PagedResponseModel<T> 
{
   public T[] Items { get; init; }
   
   public PaginationModel Pagination { get; init; }
}