using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM45;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM45;

internal sealed class SM45Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM45Request);

    public override Type FeatHandlerType => typeof(SM45Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
