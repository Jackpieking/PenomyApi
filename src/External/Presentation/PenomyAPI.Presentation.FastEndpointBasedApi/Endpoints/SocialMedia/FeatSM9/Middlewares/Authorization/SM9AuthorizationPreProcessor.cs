using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.SM9;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.Middlewares.Authorization;

internal sealed class SM9AuthorizationPreProcessor : PreProcessor<SM9RequestDto, SM9StateBag>
{
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;

    public SM9AuthorizationPreProcessor(
        TokenValidationParameters tokenValidationParameters,
        Lazy<SecurityTokenHandler> securityTokenHandler
    )
    {
        _tokenValidationParameters = tokenValidationParameters;
        _securityTokenHandler = securityTokenHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<SM9RequestDto> context,
        SM9StateBag state,
        CancellationToken ct
    )
    {
        // Bypass if response has started.
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        #region PreValidateAccessToken
        // Validate access token.
        var validateTokenResult = await _securityTokenHandler.Value.ValidateTokenAsync(
            context.HttpContext.Request.Headers.Authorization[default].Split(" ")[1],
            _tokenValidationParameters
        );

        // Extract and convert access token expire time.
        var tokenExpireTime = ExtractUtcTimeFromToken(context.HttpContext);

        // Validate access token.
        if (!validateTokenResult.IsValid || tokenExpireTime <= DateTime.UtcNow)
        {
            await SendResponseAsync(
                SM9ResponseStatusCode.UN_AUTHORIZED,
                state.AppRequest,
                context.HttpContext,
                ct
            );

            return;
        }
        #endregion

        // Get refresh token id.
        var userId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        // Save found refresh token id to state bag.
        state.AppRequest.SetUserId(userId);
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
        SM9ResponseStatusCode statusCode,
        SM9RequestDto appRequest,
        HttpContext context,
        CancellationToken ct
    )
    {
        var httpResponse = SM9HttpResponseManager
            .Resolve(statusCode)
            .Invoke(new() { StatusCode = statusCode });

        return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    }

    /// <summary>
    ///     Extract utc time from token.
    /// </summary>
    /// <param name="context">
    ///     The context containe user info.
    /// </param>
    /// <returns>
    ///     The utc time from token.
    /// </returns>
    private static DateTime ExtractUtcTimeFromToken(HttpContext context)
    {
        return DateTimeOffset
            .FromUnixTimeSeconds(
                long.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Exp))
            )
            .UtcDateTime;
    }
}
