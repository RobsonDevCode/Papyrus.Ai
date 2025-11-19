using Papyrus.Domain.Models.Explanation;

namespace Papyrus.Domain.Services.Explanation;

public interface IExplanationReaderService
{
    Task<string> GetAsync(CreateExplanationRequestModel request, CancellationToken cancellationToken);
}