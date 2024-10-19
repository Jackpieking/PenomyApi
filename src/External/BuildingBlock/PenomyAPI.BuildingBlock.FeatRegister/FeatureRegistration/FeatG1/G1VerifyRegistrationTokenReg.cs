using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG1;

internal sealed class G1VerifyRegistrationTokenReg
    : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G1VerifyRegistrationTokenRequest);

    public override Type FeatHandlerType => typeof(G1VerifyRegistrationTokenHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
        // More details: https://stackoverflow.com/questions/57998262/why-is-claimtypes-nameidentifier-not-mapping-to-sub
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();
    }
}
