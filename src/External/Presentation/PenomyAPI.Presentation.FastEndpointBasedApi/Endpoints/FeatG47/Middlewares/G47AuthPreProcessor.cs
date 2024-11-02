using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG47;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G47.Middlewares;

public class G47AuthPreProcessor : PreProcessor<G47RequestDto, G47StateBag>
{
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public G47AuthPreProcessor(
        TokenValidationParameters tokenValidationParameters,
        Lazy<SecurityTokenHandler> securityTokenHandler
    )
    {
        _tokenValidationParameters = tokenValidationParameters;
        _securityTokenHandler = securityTokenHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G47RequestDto> context,
        G47StateBag state,
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
                G47ResponseStatusCode.UNAUTHORIZED,
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
                G47ResponseStatusCode.UNAUTHORIZED,
                state.AppRequest,
                context.HttpContext,
                ct
            );
        // Save found refresh token id to state bag.
        state.AppRequest.SetUserId(id);
    }

    private static Task SendResponseAsync(
        G47ResponseStatusCode statusCode,
        G47Request appRequest,
        HttpContext context,
        CancellationToken ct
    )
    {
        var httpResponse = G47HttpResponseManager
            .Resolve(statusCode)
            .Invoke(appRequest, new G47Response { AppCode = statusCode });

        return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    }
}
