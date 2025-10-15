using Microsoft.Extensions.Logging;
using Papyrus.Domain.Exceptions;
using Papyrus.Domain.Mappers;
using Papyrus.Domain.Models.User;
using Papyrus.Domain.Services.Interfaces.User;
using Papyrus.Perstistance.Interfaces.Reader;

namespace Papyrus.Domain.Services.User;

public sealed class UserReaderService : IUserReaderService
{
    private readonly IUserReader _userReader;
    private readonly IMapper _mapper;
    private readonly ILogger<UserReaderService> _logger;

    public UserReaderService(IUserReader userReader, IMapper mapper, ILogger<UserReaderService> logger)
    {
        _userReader = userReader;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<UserModel> LoginAsync(string? username, string? email, string password,
        CancellationToken cancellationToken)
    {
        var user = await _userReader.GetAsync(username, email, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException($"{username} {email} is not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            _logger.LogError("User username: {username}, email:{email} inputted incorrect password", username, email);
            throw new InvalidOperationException("Incorrect password!");
        }

        return _mapper.MapToDomain(user);
    }
}