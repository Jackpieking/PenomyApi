using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG49;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG49.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.Middlewares;

public class G49AuthPreProcessor : PreProcessor<G49RequestDto, G49StateBag>
{
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public G49AuthPreProcessor(
        TokenValidationParameters tokenValidationParameters,
        Lazy<SecurityTokenHandler> securityTokenHandler
    )
    {
        _tokenValidationParameters = tokenValidationParameters;
        _securityTokenHandler = securityTokenHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G49RequestDto> context,
        G49StateBag state,
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
                G49ResponseStatusCode.UNAUTHORIZED,
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
                G49ResponseStatusCode.UNAUTHORIZED,
                state.AppRequest,
                context.HttpContext,
                ct
            );
        // Save found refresh token id to state bag.
        state.AppRequest.SetUserId(id);
    }

    private static Task SendResponseAsync(
        G49ResponseStatusCode statusCode,
        G49Request appRequest,
        HttpContext context,
        CancellationToken ct
    )
    {
        var httpResponse = G49HttpResponseManager
            .Resolve(statusCode)
            .Invoke(appRequest, new G49Response { AppCode = statusCode });

        return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    }
}
