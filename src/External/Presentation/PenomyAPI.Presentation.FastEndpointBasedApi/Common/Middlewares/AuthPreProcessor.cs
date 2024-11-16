using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;

internal sealed class AuthPreProcessor<TRequest> : PreProcessor<TRequest, StateBag> where TRequest : class
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<TRequest> context,
        StateBag state,
        CancellationToken ct
    )
    {
        #region PreValidateAccessToken
        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Is token expired.
        if (tokenExpireTime < DateTime.UtcNow)
        {
            await context.HttpContext.Response.SendAsync(
                new AppHttpResponse<string>
                {
                    HttpCode = StatusCodes.Status403Forbidden,
                    Body = "User token forbidden or expired"
                },
                StatusCodes.Status403Forbidden,
                null,
                ct
            );

            return;
        }

        #endregion
        // Get user id
        var userId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!long.TryParse(userId, out var id))
        {
            await context.HttpContext.Response.SendAsync(
                new AppHttpResponse<string>
                {
                    HttpCode = StatusCodes.Status401Unauthorized,
                    Body = "The user's ID is invalid"
                },
                StatusCodes.Status401Unauthorized,
                null,
                ct
            );

            return;
        }

        // Save found user id to state bag.
        state.AppRequest.UserId = id;
    }
}
