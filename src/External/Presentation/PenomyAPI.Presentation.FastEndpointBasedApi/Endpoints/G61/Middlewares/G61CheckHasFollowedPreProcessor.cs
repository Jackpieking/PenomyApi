using FastEndpoints;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G61.Middlewares;

public class G61CheckHasFollowedPreProcessor : PreProcessor<G61CheckHasFollowRequestDto, G61StateBag>
{
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public G61CheckHasFollowedPreProcessor(
        TokenValidationParameters tokenValidationParameters,
        Lazy<SecurityTokenHandler> securityTokenHandler
    )
    {
        _tokenValidationParameters = tokenValidationParameters;
        _securityTokenHandler = securityTokenHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G61CheckHasFollowRequestDto> context,
        G61StateBag state,
        CancellationToken ct
    )
    {
        // Bypass if response has started.
        if (context.HttpContext.ResponseStarted()) return;

        // Check if the request has access token or not to resolve different.
        var accessToken = context.Request.AccessToken;

        const int MINIMUM_TOKEN_LENGTH = 10;

        var hasToken = !string.IsNullOrEmpty(accessToken)
            && accessToken.Length > MINIMUM_TOKEN_LENGTH;

        // If no token is passed, then resolve for guest user.
        if (!hasToken)
        {
            state.AsGuestUser();

            return;
        }

        var tokenValidationResult = await ValidateTokenAsync(accessToken, ct);

        // If the token is invalid, then resolve the request as for guest user.
        if (!tokenValidationResult.IsValid)
        {
            state.AsGuestUser();

            return;
        }

        // If the token is valid, then resolve the request for the user who has this access token.
        var userIdClaim = tokenValidationResult.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub);

        // Check if the claim can be parsed or not. If parse failed, then resolve as guest user.
        var canParse = long.TryParse(userIdClaim.Value, out var userId);

        if (!canParse)
        {
            state.AsGuestUser();

            return;
        }

        state.AuthenticateWithUserId(userId);
    }

    private async Task<TokenValidationResult> ValidateTokenAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        // Check if the access token is using Bearer format or not.
        const string BearerPrefix = "Bearer ";

        if (accessToken.Contains(BearerPrefix))
        {
            accessToken = accessToken.Split(BearerPrefix).LastOrDefault("null");
        }

        var tokenHandler = _securityTokenHandler.Value;

        TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(
            accessToken,
            _tokenValidationParameters);

        return validationResult;
    }
}
