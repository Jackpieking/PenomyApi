using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM12.OtherHandler;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM12;

internal sealed class SM12OtherReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM12OtherRequest);

    public override Type FeatHandlerType => typeof(SM12OtherHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
