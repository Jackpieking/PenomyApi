using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadPublicLevels;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt4;

internal sealed class Art4LoadPublicLevelReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art4LoadPublicLevelRequest);

    public override Type FeatHandlerType => typeof(Art4LoadPublicLevelHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
