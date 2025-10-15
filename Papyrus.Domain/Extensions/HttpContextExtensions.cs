using Microsoft.AspNetCore.Http;

namespace Papyrus.Domain.Extensions;

public static class HttpContextExtensions
{
    public static void AddJwt(this HttpContext httpContext, string jwtToken, DateTime expires)
    {
        httpContext.Response.Cookies.Append("jwt", jwtToken, 
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires
            });
    }
}