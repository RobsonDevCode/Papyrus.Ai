using System.Security.Claims;
using Exception = System.Exception;

namespace Papyrus.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var userIdAsString =  principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdAsString))
        {
            return null;
        }

        if (!Guid.TryParse(userIdAsString, out var userId))
        {
            throw new Exception($"Invalid user id {userIdAsString} found in claims");
        }
        
        return userId;
    }
}