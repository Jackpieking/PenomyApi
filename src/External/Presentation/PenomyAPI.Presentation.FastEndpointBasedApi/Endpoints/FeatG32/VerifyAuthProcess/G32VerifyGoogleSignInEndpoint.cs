using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.FeatG32.OtherHandlers.VerifyGoogleSignIn;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG32.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG32.VerifyAuthProcess;

public class G32VerifyGoogleSignInEndpoint : EndpointWithoutRequest
{
    private readonly Lazy<SignInManager<PgUser>> _signInManager;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly Lazy<SecurityTokenHandler> _securityTokenHandler;
    private readonly GoogleSignInOption _googleSignInOptions;

    public G32VerifyGoogleSignInEndpoint(
        Lazy<SignInManager<PgUser>> signInManager,
        TokenValidationParameters tokenValidationParameters,
        Lazy<SecurityTokenHandler> securityTokenHandler,
        GoogleSignInOption googleSignInOptions
    )
    {
        _signInManager = signInManager;
        _tokenValidationParameters = tokenValidationParameters;
        _securityTokenHandler = securityTokenHandler;
        _googleSignInOptions = googleSignInOptions;
    }

    public override void Configure()
    {
        Get("g32/verify-google-login");
        AuthSchemes(GoogleDefaults.AuthenticationScheme);
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for verify google signin feature";
            summary.Description = "This endpoint is used for verify google signin purpose.";
        });
    }

    public override async Task<object> ExecuteAsync(CancellationToken ct)
    {
        const string InValidAuthStateMessage =
            "Cannot process google login. Invalid auth state value.";

        // Validate auth state.
        var (userId, email, nickName, userGoogleId) = await ValidateAndGetUserIdFromAuthStateAsync(
            ct
        );

        // Auth state is invalid.
        if (userId == long.MinValue)
        {
            await SendAsync(InValidAuthStateMessage, StatusCodes.Status422UnprocessableEntity, ct);

            return InValidAuthStateMessage;
        }

        // Construct internal app request.
        var appRequest = new G32VerifyGoogleSignInRequest
        {
            UserId = userId,
            Email = email,
            NickName = nickName,
            UserGoogleId = userGoogleId
        };

        // Execute main logic.
        var appResponse = await FeatureExtensions.ExecuteAsync<
            G32VerifyGoogleSignInRequest,
            G32VerifyGoogleSignInResponse
        >(appRequest, ct);

        var redirectUrl = string.Empty;

        // if success, create redirect url with access token, refresh token, app code.
        if (appResponse.StatusCode == G32VerifyGoogleSignInResponseStatusCode.SUCCESS)
        {
            redirectUrl = ConstructFinalRedirectUrl(
                _googleSignInOptions.Verify.ResponseRedirectBaseUrl,
                appResponse.Body.AccessToken,
                WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(appResponse.Body.RefreshToken)),
                appResponse.StatusCode.ToString()
            );
        }
        else
        {
            redirectUrl = ConstructFinalRedirectUrl(
                _googleSignInOptions.Verify.ResponseRedirectBaseUrl,
                string.Empty,
                string.Empty,
                appResponse.StatusCode.ToString()
            );
        }

        HttpContext.Response.Cookies.Delete(G32Common.AuthStateCookieName);
        await _signInManager.Value.SignOutAsync();

        await SendRedirectAsync(redirectUrl, true, true);

        return string.Empty;
    }

    private static string ConstructFinalRedirectUrl(
        string baseUrl,
        string accessToken,
        string refreshToken,
        string appCode
    )
    {
        var stringHandler = new DefaultInterpolatedStringHandler();

        stringHandler.AppendFormatted(baseUrl);
        stringHandler.AppendLiteral("?");
        stringHandler.AppendFormatted("access_token=");
        stringHandler.AppendFormatted(accessToken);
        stringHandler.AppendLiteral("&");
        stringHandler.AppendFormatted("refresh_token=");
        stringHandler.AppendFormatted(refreshToken);
        stringHandler.AppendLiteral("&");
        stringHandler.AppendFormatted("app_code=");
        stringHandler.AppendFormatted(appCode);

        return stringHandler.ToStringAndClear();
    }

    private async Task<(long, string, string, string)> ValidateAndGetUserIdFromAuthStateAsync(
        CancellationToken ct
    )
    {
        // try get auth state value in cookie.
        var isAuthStateInCookieFound = HttpContext.Request.Cookies.TryGetValue(
            G32Common.AuthStateCookieName,
            out var authStateValue
        );

        // if not found, this request from google is not made by server, so invalid.
        if (!isAuthStateInCookieFound)
        {
            return (long.MinValue, string.Empty, string.Empty, string.Empty);
        }

        // Decrypt the jwt [auth state value] and get user id
        var newUserId = await VerifyJwtFoundInCookieAndGetUserIdAsync(authStateValue);

        // Cannot find user id in the auth state value in cookie [maybe fake by hacker?].
        if (string.IsNullOrWhiteSpace(newUserId))
        {
            return (long.MinValue, string.Empty, string.Empty, string.Empty);
        }

        // Try to get the login info from last login with
        // the notification that csrf is enable.
        var loginInfo = await _signInManager.Value.GetExternalLoginInfoAsync(newUserId);

        // if not found, this request from google is not made by server, so invalid.
        if (Equals(loginInfo, null))
        {
            return (long.MinValue, string.Empty, string.Empty, string.Empty);
        }

        var userIdAsLong = long.Parse(newUserId);

        var userEmail = loginInfo
            .Principal.Claims.First(claim => claim.Type.Equals(ClaimTypes.Email))
            .Value;

        var userNickname = loginInfo
            .Principal.Claims.First(claim => claim.Type.Equals(ClaimTypes.Name))
            .Value;

        var userGoogleId = loginInfo.ProviderKey;

        return (long.Parse(newUserId), userEmail, userNickname, userGoogleId);
    }

    public async Task<string> VerifyJwtFoundInCookieAndGetUserIdAsync(string token)
    {
        var validationResult = await _securityTokenHandler.Value.ValidateTokenAsync(
            token,
            _tokenValidationParameters
        );

        // Valdate the token fail.
        if (!validationResult.IsValid)
        {
            return string.Empty;
        }

        // Get expiration time.
        var expireTime = DateTimeOffset
            .FromUnixTimeSeconds(
                seconds: long.Parse(
                    validationResult.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Exp).Value
                )
            )
            .UtcDateTime;

        // Is token expired?
        if (expireTime < DateTime.UtcNow)
        {
            return string.Empty;
        }

        var isRightPurpose = validationResult.ClaimsIdentity.HasClaim(claim =>
            claim.Type.Equals(CommonValues.Claims.TokenPurpose.Type)
            && claim.Value.Equals(CommonValues.Claims.TokenPurpose.Values.GoogleSignin)
        );

        // Token is not for verify email.
        if (!isRightPurpose)
        {
            return string.Empty;
        }

        // Return user id.
        return validationResult.ClaimsIdentity.FindFirst(CommonValues.Claims.UserIdClaim)?.Value;
    }
}
