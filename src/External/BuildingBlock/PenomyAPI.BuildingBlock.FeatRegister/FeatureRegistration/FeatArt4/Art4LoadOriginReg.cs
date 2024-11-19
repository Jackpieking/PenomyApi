using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadOrigin;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt4;

internal sealed class Art4LoadOriginReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art4LoadOriginRequest);

    public override Type FeatHandlerType => typeof(Art4LoadOriginHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
