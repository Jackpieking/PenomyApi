using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Identity.AppAuthenticator;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

internal sealed class IdentityServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        AddAuthenticatingServices(services, configuration);

        AddAppDefinedServices(services);
    }

    private static void AddAppDefinedServices(IServiceCollection services)
    {
        services
            .AddSingleton<IAccessTokenHandler, AccessTokenHandler>()
            .MakeSingletonLazy<IAccessTokenHandler>()
            .AddSingleton<IRefreshTokenHandler, RefreshTokenHandler>()
            .MakeSingletonLazy<IRefreshTokenHandler>()
            .AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>()
            .MakeSingletonLazy<SecurityTokenHandler>();
    }

    private static void AddAuthenticatingServices(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var appAuthOption = configuration
            .GetRequiredSection("Authentication")
            .GetRequiredSection("Jwt")
            .Get<JwtAuthenticationOptions>();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = appAuthOption.ValidateIssuer,
            ValidateAudience = appAuthOption.ValidateAudience,
            ValidateLifetime = appAuthOption.ValidateLifetime,
            ValidateIssuerSigningKey = appAuthOption.ValidateIssuerSigningKey,
            RequireExpirationTime = appAuthOption.RequireExpirationTime,
            ValidTypes = appAuthOption.ValidTypes,
            ValidIssuer = appAuthOption.ValidIssuer,
            ValidAudience = appAuthOption.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                new HMACSHA256(Encoding.UTF8.GetBytes(appAuthOption.IssuerSigningKey)).Key
            )
        };

        var adminAuthOption = configuration
            .GetRequiredSection("Authentication")
            .GetRequiredSection("AdminJwt")
            .Get<AdminJwtAuthenticationOptions>();
        var adminTokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = adminAuthOption.ValidateIssuer,
            ValidateAudience = adminAuthOption.ValidateAudience,
            ValidateLifetime = adminAuthOption.ValidateLifetime,
            ValidateIssuerSigningKey = adminAuthOption.ValidateIssuerSigningKey,
            RequireExpirationTime = adminAuthOption.RequireExpirationTime,
            ValidTypes = adminAuthOption.ValidTypes,
            ValidIssuer = adminAuthOption.ValidIssuer,
            ValidAudience = adminAuthOption.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                new HMACSHA256(Encoding.UTF8.GetBytes(adminAuthOption.IssuerSigningKey)).Key
            )
        };

        var googleAuthOption = configuration
            .GetRequiredSection("Authentication")
            .GetRequiredSection("Google")
            .Get<GoogleAuthenticationOption>();

        // Revert back to default authentication, adandon fast endpoints.
        services
            .AddSingleton(tokenValidationParameters)
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config => config.TokenValidationParameters = tokenValidationParameters)
            .AddJwtBearer(
                "AdminJwt",
                config => config.TokenValidationParameters = adminTokenValidationParameters
            )
            .AddGoogle(
                GoogleDefaults.AuthenticationScheme,
                config =>
                {
                    config.ClientId = googleAuthOption.ClientId;
                    config.ClientSecret = googleAuthOption.ClientSecret;
                    config.CallbackPath = googleAuthOption.CallBackPath;
                    config.SignInScheme = IdentityConstants.ExternalScheme;
                    config.Events.OnRemoteFailure = async context =>
                    {
                        await context.Response.SendRedirectAsync(
                            googleAuthOption.InitLoginPath,
                            true,
                            true
                        );

                        context.HandleResponse();
                    };
                }
            );

        services.AddAuthorization();

        // services
        //     .AddSingleton(tokenValidationParameters)
        //     .AddAuthenticationJwtBearer(
        //         jwtSigningOption =>
        //         {
        //             jwtSigningOption.SigningKey = authOption.IssuerSigningKey;
        //         },
        //         jwtBearerOption =>
        //         {
        //             jwtBearerOption.TokenValidationParameters = tokenValidationParameters;

        //             jwtBearerOption.Validate();
        //         }
        //     )
        //     .AddAuthorization();
    }
}
