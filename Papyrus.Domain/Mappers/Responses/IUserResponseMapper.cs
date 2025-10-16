using Papyrus.Api.Contracts.Contracts.Responses;
using Papyrus.Domain.Models.User;

namespace Papyrus.Domain.Mappers.Responses;

public interface IUserResponseMapper
{
    UserResponse MapToResponse(UserModel user);
}