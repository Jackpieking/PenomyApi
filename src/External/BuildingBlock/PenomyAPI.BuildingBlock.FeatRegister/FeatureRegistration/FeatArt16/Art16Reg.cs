using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt16;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt16;

internal class Art16Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art16Request);

    public override Type FeatHandlerType => typeof(Art16Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
