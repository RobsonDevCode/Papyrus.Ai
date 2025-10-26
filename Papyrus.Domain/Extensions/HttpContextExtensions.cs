using Microsoft.AspNetCore.Http;

namespace Papyrus.Domain.Extensions;

public static class HttpContextExtensions
{
    public static void AddJwt(this HttpContext httpContext, string jwtToken, DateTime expires, string? refreshToken = null)
    {
        httpContext.Response.Cookies.Append("jwt", jwtToken, 
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires
            });

        if (!string.IsNullOrEmpty(refreshToken))
        {
            httpContext.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(7),
                Path = "/user/refresh-token"
            });
        }
    }
}