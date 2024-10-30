using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG33;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.Middlewares.Authorization;

internal sealed class G33AuthorizationPreProcessor : PreProcessor<EmptyRequest, G33StateBag>
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<EmptyRequest> context,
        G33StateBag state,
        CancellationToken ct
    )
    {
        #region PreValidateAccessToken
        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Validate access token.
        if (tokenExpireTime < DateTime.UtcNow)
        {
            await SendResponseAsync(
                G33ResponseStatusCode.UN_AUTHORIZED,
                state.AppRequest,
                context.HttpContext,
                ct
            );

            return;
        }
        #endregion
        // Get access token id, also is the refresh token id.
        var tokenId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        // Save found refresh token id to state bag.
        state.AppRequest.SetRefreshTokenId(tokenId);
    }

    /// <summary>
    ///     Send response extension method.
    /// </summary>
    /// <param name="statusCode">
    ///     The app status code.
    /// </param>
    /// <param name="appRequest">
    ///     The app request.
    /// </param>
    /// <param name="context">
    ///     The http context.
    /// </param>
    /// <param name="ct">
    ///     The cancellation token used to propagate
    ///     notification that operations should be canceled.
    /// </param>
    /// <returns>
    ///     Empty
    /// </returns>
    private static Task SendResponseAsync(
        G33ResponseStatusCode statusCode,
        G33Request appRequest,
        HttpContext context,
        CancellationToken ct
    )
    {
        var httpResponse = G33HttpResponseMapper
            .Resolve(statusCode)
            .Invoke(appRequest, new() { StatusCode = statusCode });

        return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    }
}
