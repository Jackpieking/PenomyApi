using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG1;

internal sealed class G1VerifyRegistrationTokenReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G1VerifyRegistrationTokenRequest);

    public override Type FeatHandlerType => typeof(G1VerifyRegistrationTokenHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
