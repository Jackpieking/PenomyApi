using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM23;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM23;

internal sealed class SM23Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM23Request);

    public override Type FeatHandlerType => typeof(SM23Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
