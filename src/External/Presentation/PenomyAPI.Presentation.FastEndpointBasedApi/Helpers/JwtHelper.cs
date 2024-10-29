using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;

public static class JwtHelper
{
    /// <summary>
    ///     Extract utc time from token.
    /// </summary>
    /// <param name="context">
    ///     The context containe user info.
    /// </param>
    /// <returns>
    ///     The utc time from token.
    /// </returns>
    public static DateTime ExtractUtcTimeFromToken(HttpContext context)
    {
        return DateTimeOffset
            .FromUnixTimeSeconds(
                long.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Exp))
            )
            .UtcDateTime;
    }

    /// <summary>
    ///     Is access token expired.
    /// </summary>
    /// <param name="context">
    ///     The context containe user info.
    /// </param>
    /// <returns>
    ///     True if token is expired.
    ///     False if token is not expired.
    /// </returns>
    public static bool IsAccessTokenExpired(HttpContext context)
    {
        var tokenExpireTime = DateTimeOffset
            .FromUnixTimeSeconds(
                long.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Exp))
            )
            .UtcDateTime;

        return tokenExpireTime <= DateTime.UtcNow;
    }
}
