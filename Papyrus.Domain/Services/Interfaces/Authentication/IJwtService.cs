using Microsoft.AspNetCore.Http;
using Papyrus.Domain.Models.Authentication;

namespace Papyrus.Domain.Services.Interfaces.Authentication;

public interface IJwtService
{
    Task<JwtModel> GenerateJwtToken(Guid userId, string username, string email,
        IEnumerable<string>? roles = null, CancellationToken cancellationToken = default);

    Task<JwtModel> ReGenerateJwtAsync(HttpContext httpContext, CancellationToken cancellationToken);
}