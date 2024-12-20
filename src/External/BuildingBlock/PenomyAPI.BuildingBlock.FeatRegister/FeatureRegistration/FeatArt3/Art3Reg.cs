using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt3;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt3;

internal sealed class Art3Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art3Request);

    public override Type FeatHandlerType => typeof(Art3Handler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
