using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG5.OtherHandlers.GetArtworkMetaData;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG5;

internal class G5ArtworkMetaDataReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G5ArtworkMetaDataRequest);

    public override Type FeatHandlerType => typeof(G5ArtworkMetaDataHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}