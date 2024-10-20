using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.Infra.Configuration.Options;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Infra.FeatG1;

public sealed class G1PreRegistrationTokenHandler
    : IG1PreRegistrationTokenHandler
{
    private readonly JwtAuthenticationOptions _options;
    private readonly SecurityTokenHandler _securityTokenHandler;

    public G1PreRegistrationTokenHandler(
        JwtAuthenticationOptions options,
        SecurityTokenHandler securityTokenHandler)
    {
        _options = options;
        _securityTokenHandler = securityTokenHandler;
    }

    public async Task<Result<string>> GetEmailFromTokenAsync(
        string token,
        CancellationToken cancellationToken)
    {
        var validationResult = await ValidateTokenParametersAsync(token);

        if (!validationResult.IsValid)
        {
            return Result<string>.Failed();
        }

        var emailClaim = validationResult.ClaimsIdentity.FindFirst(ClaimTypes.Email);

        return Result<string>.Success(emailClaim.Value);
    }

    /// <summary>
    ///     Validate the parameters of the input token is valid
    ///     to this application's requirements or not.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<TokenValidationResult> ValidateTokenParametersAsync(string token)
    {
        // Validate the token credentials.
        var validationResult = await _securityTokenHandler.ValidateTokenAsync(
            token: token,
            validationParameters: new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.ValidIssuer,
                ValidAudience = _options.ValidAudience,
                IssuerSigningKey = GetSecurityKey(_options.IssuerSigningKey)
            }
        );

        return validationResult;
    }

    private static SymmetricSecurityKey GetSecurityKey(string PrivateKey)
    {
        var key = Encoding.UTF8.GetBytes(PrivateKey);

        return new SymmetricSecurityKey(key: key);
    }
}
