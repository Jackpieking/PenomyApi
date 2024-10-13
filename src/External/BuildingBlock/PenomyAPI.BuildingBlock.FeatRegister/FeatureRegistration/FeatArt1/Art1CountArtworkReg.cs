using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt1.OtherHandlers;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt1;

internal sealed class Art1CountArtworkReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art1CountArtworkRequest);

    public override Type FeatHandlerType => typeof(Art1CountArtworkHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
