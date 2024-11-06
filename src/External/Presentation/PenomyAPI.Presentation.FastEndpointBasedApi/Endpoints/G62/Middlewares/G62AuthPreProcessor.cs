using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using PenomyAPI.App.G62;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.Middlewares;

public class G62AuthPreProcessor : PreProcessor<G62RequestDto, G62StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<G62RequestDto> context,
        G62StateBag state,
        CancellationToken ct
    )
    {
        // Bypass if response has started.
        if (context.HttpContext.ResponseStarted()) return;

        #region PreValidateAccessToken

        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Validate access token.
        if (tokenExpireTime <= DateTime.UtcNow)
        {
            await SendResponseAsync(
                G62ResponseStatusCode.FORBIDDEN,
                state.AppRequest,
                context.HttpContext,
                ct
            );

            return;
        }

        #endregion

        // Get refresh token id.
        var userId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!long.TryParse(userId, out var id))
            await SendResponseAsync(
                G62ResponseStatusCode.UN_AUTHORIZED,
                state.AppRequest,
                context.HttpContext,
                ct
            );

        // Save found refresh token id to state bag.
        state.AppRequest.UserId = id;
    }

    private static Task SendResponseAsync(
        G62ResponseStatusCode statusCode,
        G62Request appRequest,
        HttpContext context,
        CancellationToken ct
    )
    {
        var httpResponse = G62ResponseManager
            .Resolve(statusCode)
            .Invoke(appRequest, new G62Response { StatusCode = statusCode });

        return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    }
}
