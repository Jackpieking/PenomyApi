using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.FeatG31A;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG31A.Middlewares.Authorization;

internal sealed class G31AAuthorizationPreProcessor : PreProcessor<G31ARequest, G31AStateBag>
{
    private readonly Lazy<IServiceScopeFactory> _serviceScopeFactory;

    public G31AAuthorizationPreProcessor(Lazy<IServiceScopeFactory> serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G31ARequest> context,
        G31AStateBag state,
        CancellationToken ct
    )
    {
        // Skip if the previous has send a response.
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        #region PreValidateAccessToken
        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Validate access token.
        if (tokenExpireTime > DateTime.UtcNow)
        {
            await SendResponseAsync(
                G31AResponseStatusCode.FORBIDDEN,
                context.Request,
                context.HttpContext,
                ct
            );

            return;
        }
        #endregion

        await using var scope = _serviceScopeFactory.Value.CreateAsyncScope();

        var repository = scope.Resolve<Lazy<IUnitOfWork>>().Value.G31ARepository;

        // Get access token id, also is the refresh token id.
        var accessTokenId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        // Is refresh token found and not expired [both token use same id].
        var isRefreshTokenFound = await repository.IsRefreshTokenFoundAsync(
            accessTokenId,
            context.Request.RefreshToken,
            ct
        );

        // Refresh token is not found.
        if (!isRefreshTokenFound)
        {
            await SendResponseAsync(
                G31AResponseStatusCode.FORBIDDEN,
                context.Request,
                context.HttpContext,
                ct
            );

            return;
        }

        // Is refresh token expired.
        var isRefreshTokenExpired = await repository.IsRefreshTokenExpiredAsync(accessTokenId, ct);

        // Refresh token is expired.
        if (isRefreshTokenExpired)
        {
            await SendResponseAsync(
                G31AResponseStatusCode.UN_AUTHORIZED,
                context.Request,
                context.HttpContext,
                ct
            );

            return;
        }

        // Save found access token id to state bag.
        context.Request.SetAccessTokenId(accessTokenId);

        // Get user id.
        var userId = context.HttpContext.User.FindFirstValue(CommonValues.Claims.UserIdClaim);
        context.Request.SetUserId(userId);
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
        G31AResponseStatusCode statusCode,
        G31ARequest appRequest,
        HttpContext context,
        CancellationToken ct
    )
    {
        var httpResponse = G31AHttpResponseMapper
            .Resolve(statusCode)
            .Invoke(appRequest, new() { StatusCode = statusCode });

        return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    }
}
