using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.Infra.Configuration.Options;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG32.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG32.InitAuthProcess;

public sealed class G32Endpoint : EndpointWithoutRequest
{
    private readonly Lazy<SignInManager<PgUser>> _signInManager;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly Lazy<IAccessTokenHandler> _accessTokenHandler;
    private readonly GoogleSignInOption _googleSignInOptions;
    private const int AuthValidDuration = 60;
    private static readonly CookieOptions AuthStateCookieOption =
        new()
        {
            Path = "/",
            Domain = "localhost",
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddSeconds(AuthValidDuration),
            IsEssential = true,
            MaxAge = TimeSpan.FromSeconds(AuthValidDuration),
            Secure = true,
            SameSite = SameSiteMode.Lax
        };

    public G32Endpoint(
        Lazy<SignInManager<PgUser>> signInManager,
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IAccessTokenHandler> accessTokenHandler,
        GoogleSignInOption googleSignInOptions
    )
    {
        _signInManager = signInManager;
        _idGenerator = idGenerator;
        _accessTokenHandler = accessTokenHandler;
        _googleSignInOptions = googleSignInOptions;
    }

    public override void Configure()
    {
        Get("g32/signin-google");
        AllowAnonymous();
        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
        });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for init google signin feature";
            summary.Description = "This endpoint is used for init google signin purpose.";
        });
    }

    public override async Task<object> ExecuteAsync(CancellationToken ct)
    {
        // Generate user id
        var userId = _idGenerator.Value.Get().ToString();

        // Generate authentication properties
        var authPropperties = _signInManager.Value.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme,
            _googleSignInOptions.Init.ResponseRedirectUrl,
            userId
        );

        // Set some more properties.
        authPropperties.IssuedUtc = DateTime.UtcNow;
        authPropperties.ExpiresUtc = DateTime.UtcNow.AddSeconds(AuthValidDuration);
        authPropperties.AllowRefresh = false;
        authPropperties.IsPersistent = false;

        // Generating state as jwt containing user id.
        var authStateValueAsJwt = _accessTokenHandler.Value.Generate(
            [new(CommonValues.Claims.UserIdClaim, userId)],
            AuthValidDuration
        );

        // Set auth state to cookie.
        HttpContext.Response.Cookies.Append(
            G32Common.AuthStateCookieName,
            authStateValueAsJwt,
            AuthStateCookieOption
        );

        // Send back challenge.
        await SendResultAsync(
            Results.Challenge(authPropperties, [GoogleDefaults.AuthenticationScheme])
        );

        return string.Empty;
    }
}
