using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt15;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt15;

internal sealed class Art15Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art15Request);

    public override Type FeatHandlerType => typeof(Art15Handler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
