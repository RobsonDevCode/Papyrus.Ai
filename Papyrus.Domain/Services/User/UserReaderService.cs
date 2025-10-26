using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;
    private readonly ILogger<UserReaderService> _logger;

    public UserReaderService(IUserReader userReader,
        IMemoryCache memoryCache,
        IMapper mapper,
        ILogger<UserReaderService> logger)
    {
        _userReader = userReader;
        _memoryCache = memoryCache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserModel> LoginAsync(string email, string password,
        CancellationToken cancellationToken)
    {
        var user = await _userReader.GetAsync(email, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException($"{email} is not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            _logger.LogError("User username: {username}, email:{email} inputted incorrect password", user.Username,
                email);
            throw new InvalidOperationException("Incorrect password!");
        }

        return _mapper.MapToDomain(user);
    }

    public async ValueTask<UserModel?> GetById(Guid userId, CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync(userId, async entry =>
        {
            var user = await _userReader.GetAsync(userId, cancellationToken);
            return user is null ? null : _mapper.MapToDomain(user);
        });
    }
}