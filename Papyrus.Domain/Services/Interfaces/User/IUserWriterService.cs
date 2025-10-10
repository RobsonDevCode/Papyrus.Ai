using Papyrus.Domain.Models.User;

namespace Papyrus.Domain.Services.Interfaces.User;

public interface IUserWriterService
{
    Task<UserModel> CreateAsync(CreateUserRequestModel request, CancellationToken cancellationToken); 
}