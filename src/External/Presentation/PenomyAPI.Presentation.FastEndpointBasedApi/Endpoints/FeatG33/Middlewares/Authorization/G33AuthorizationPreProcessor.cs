using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG33;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.HttpResponseManager;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG33.Middlewares.Authorization;

internal sealed class G33AuthorizationPreProcessor : PreProcessor<EmptyRequest, G33StateBag>
{
    private readonly Lazy<IServiceScopeFactory> _serviceScopeFactory;

    public G33AuthorizationPreProcessor(Lazy<IServiceScopeFactory> serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

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

        await using var scope = _serviceScopeFactory.Value.CreateAsyncScope();

        var repository = scope.Resolve<Lazy<IUnitOfWork>>().Value.G33Repository;

        // Get access token id..
        var tokenId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

        // Is refresh token found by access token id [both token use same id].
        var isRefreshTokenFound = await repository.IsRefreshTokenFoundByIdAsync(tokenId, ct);

        // Refresh token is not found.
        if (!isRefreshTokenFound)
        {
            await SendResponseAsync(
                G33ResponseStatusCode.FORBIDDEN,
                state.AppRequest,
                context.HttpContext,
                ct
            );

            return;
        }

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
