using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using FastEndpoints.Security;
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
        var authOption = configuration
            .GetRequiredSection("Authentication")
            .GetRequiredSection("Jwt")
            .Get<JwtAuthenticationOptions>();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = authOption.ValidateIssuer,
            ValidateAudience = authOption.ValidateAudience,
            ValidateLifetime = authOption.ValidateLifetime,
            ValidateIssuerSigningKey = authOption.ValidateIssuerSigningKey,
            RequireExpirationTime = authOption.RequireExpirationTime,
            ValidTypes = authOption.ValidTypes,
            ValidIssuer = authOption.ValidIssuer,
            ValidAudience = authOption.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                key: new HMACSHA256(key: Encoding.UTF8.GetBytes(s: authOption.IssuerSigningKey)).Key
            )
        };

        services
            .AddSingleton(tokenValidationParameters)
            .AddAuthenticationJwtBearer(
                jwtSigningOption =>
                {
                    jwtSigningOption.SigningKey = authOption.IssuerSigningKey;
                },
                jwtBearerOption =>
                {
                    jwtBearerOption.TokenValidationParameters = tokenValidationParameters;

                    jwtBearerOption.Validate();
                }
            )
            .AddAuthorization();
    }
}
