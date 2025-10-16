using Papyrus.Domain.Models.User;

namespace Papyrus.Domain.Services.Interfaces.User;

public interface IUserReaderService
{
    Task<UserModel> LoginAsync(string? username, string? email, string password, CancellationToken cancellationToken);
}