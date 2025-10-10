using Papyrus.Api.Contracts.Contracts.Requests;
using Papyrus.Domain.Models.User;
using Papyrus.Persistance.Interfaces.Contracts;

namespace Papyrus.Domain.Mappers.Domain;

public interface IUserDomainMapper
{
    UserModel MapToDomain(User user);
    CreateUserRequestModel MapToDomain(CreateUserRequest request);
}