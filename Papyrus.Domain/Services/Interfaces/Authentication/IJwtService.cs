namespace Papyrus.Domain.Services.Interfaces.Authentication;

public interface IJwtService
{
    (string JWT, DateTime Expires) GenerateJwtToken(Guid userId, string username, string email, IEnumerable<string>? roles = null);
}