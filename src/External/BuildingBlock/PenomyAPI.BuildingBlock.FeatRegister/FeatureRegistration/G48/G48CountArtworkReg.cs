using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G48.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G48;

internal sealed class G48CountArtworkReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G48CountArtworkRequest);

    public override Type FeatHandlerType => typeof(G48CountArtworkHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
