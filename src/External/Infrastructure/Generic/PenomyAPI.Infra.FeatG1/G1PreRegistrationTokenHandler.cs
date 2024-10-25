using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.FeatG1.Infrastructures;

namespace PenomyAPI.Infra.FeatG1;

public sealed class G1PreRegistrationTokenHandler : IG1PreRegistrationTokenHandler
{
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;

    public G1PreRegistrationTokenHandler(
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
    ///     Get email from token.
    /// </summary>
    /// <param name="token">
    ///     The token to be validated.
    /// </param>
    /// <param name="cancellationToken">
    ///     The cancellation token.
    /// </param>
    /// <returns>
    ///     The email from token.
    /// </returns>
    public async Task<string> GetEmailFromTokenAsync(
        string token,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await ValidateTokenParametersAsync(token);

        // Valdate the token fail.
        if (!validationResult.IsValid)
        {
            return string.Empty;
        }

        // Is token expired?
        if (ExtractUtcTimeFromToken(validationResult) < DateTime.UtcNow)
        {
            return string.Empty;
        }

        var isClaimFound = validationResult.ClaimsIdentity.HasClaim(claim =>
            claim.Type.Equals(CommonValues.Claims.AppUserEmailClaim)
        );

        if (!isClaimFound)
        {
            return string.Empty;
        }

        return validationResult
            .ClaimsIdentity.FindFirst(CommonValues.Claims.AppUserEmailClaim)
            .Value;
    }
}
