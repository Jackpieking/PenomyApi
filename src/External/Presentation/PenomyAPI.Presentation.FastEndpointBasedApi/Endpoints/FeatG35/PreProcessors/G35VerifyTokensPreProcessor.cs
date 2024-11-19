using FastEndpoints;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.PreProcessors;

public sealed class G35VerifyTokensPreProcessor
    : PreProcessor<G35VerifyTokensDto, G35StateBag>
{
    public override Task PreProcessAsync(
        IPreProcessorContext<G35VerifyTokensDto> context,
        G35StateBag state,
        CancellationToken ct)
    {
        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Is token expired.
        if (tokenExpireTime < DateTime.UtcNow)
        {
            return Task.CompletedTask;
        }

        // Get token purpose.
        var tokenPurpose = context.HttpContext.User.FindFirstValue(
            CommonValues.Claims.TokenPurpose.Type
        );

        // Token is not for user access.
        var isValidPurpose = tokenPurpose.Equals(
            CommonValues.Claims.TokenPurpose.Values.AppUserAccess);

        if (!isValidPurpose)
        {
            return Task.CompletedTask;
        }

        // Get the userId claim from the access-token for later operation.
        var userIdClaimValue = context.HttpContext.User.FindFirstValue(
            JwtRegisteredClaimNames.Sub);

        var canParse = long.TryParse(userIdClaimValue, out var userId);

        if (canParse)
        {
            state.IsAuthorized = true;
            state.UserId = userId;
        }

        // Get access token id, also is the refresh token id.
        state.RefreshTokenId = context.HttpContext.User.FindFirstValue(
            JwtRegisteredClaimNames.Jti);

        return Task.CompletedTask;
    }
}
