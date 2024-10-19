using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG1;

internal sealed class G1CompleteRegistrationReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G1CompleteRegistrationRequest);

    public override Type FeatHandlerType => typeof(G1CompleteRegistrationHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
