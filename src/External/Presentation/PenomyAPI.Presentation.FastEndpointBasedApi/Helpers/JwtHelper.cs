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
}
