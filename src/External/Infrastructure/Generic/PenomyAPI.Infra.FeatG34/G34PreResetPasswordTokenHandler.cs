using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.FeatG34.Infrastructures;

namespace PenomyAPI.Infra.FeatG34;

public sealed class G34PreResetPasswordTokenHandler : IG34PreResetPasswordTokenHandler
{
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;

    public G34PreResetPasswordTokenHandler(
        TokenValidationParameters tokenValidationParameters,
        Lazy<SecurityTokenHandler> securityTokenHandler
    )
    {
        _tokenValidationParameters = tokenValidationParameters;
        _securityTokenHandler = securityTokenHandler;
    }

    /// <summary>
    ///     Validate the parameters of the input token is valid
    ///     to this application's requirements or not.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private Task<TokenValidationResult> ValidateTokenParametersAsync(string token)
    {
        // Validate the token credentials.
        return _securityTokenHandler.Value.ValidateTokenAsync(token, _tokenValidationParameters);
    }

    /// <summary>
    ///     Extract utc time from token.
    /// </summary>
    /// <param name="tokenResult">
    ///     The token validation result.
    /// </param>
    /// <returns>
    ///     The utc time from token.
    /// </returns>
    private static DateTime ExtractUtcTimeFromToken(TokenValidationResult tokenResult)
    {
        return DateTimeOffset
            .FromUnixTimeSeconds(
                seconds: long.Parse(
                    tokenResult.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Exp).Value
                )
            )
            .UtcDateTime;
    }

    /// <summary>
    ///     Get token id from token.
    /// </summary>
    /// <param name="token">
    ///     The token to be validated.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     The token id from token.
    /// </returns>
    public async Task<(string, string)> GetTokenInfoFromTokenAsync(
        string token,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await ValidateTokenParametersAsync(token);

        // Valdate the token fail.
        if (!validationResult.IsValid)
        {
            return (string.Empty, string.Empty);
        }

        // Is token expired?
        if (ExtractUtcTimeFromToken(validationResult) < DateTime.UtcNow)
        {
            return (string.Empty, string.Empty);
        }

        var isRightPurpose = validationResult.ClaimsIdentity.HasClaim(claim =>
            claim.Type.Equals(CommonValues.Claims.TokenPurpose.Type)
            && claim.Value.Equals(CommonValues.Claims.TokenPurpose.Values.ResetPassword)
        );

        // Token is not for reset password.
        if (!isRightPurpose)
        {
            return (string.Empty, string.Empty);
        }

        return (
            validationResult.ClaimsIdentity.FindFirst(CommonValues.Claims.TokenIdClaim).Value,
            validationResult.ClaimsIdentity.FindFirst(CommonValues.Claims.UserIdClaim).Value
        );
    }
}
