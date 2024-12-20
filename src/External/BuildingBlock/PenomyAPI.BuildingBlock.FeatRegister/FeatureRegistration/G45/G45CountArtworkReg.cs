using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G45.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G45;

internal sealed class G45CountArtworkReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G45CountArtworkRequest);

    public override Type FeatHandlerType => typeof(G45CountArtworkHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
