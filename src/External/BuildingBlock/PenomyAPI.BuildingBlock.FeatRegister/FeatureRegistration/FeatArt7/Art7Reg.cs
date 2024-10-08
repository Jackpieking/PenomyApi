using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt7;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt7;

internal sealed class Art7Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art7Request);

    public override Type FeatHandlerType => typeof(Art7Handler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
