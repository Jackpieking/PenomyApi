using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt17.OtherHandlers.GetDetail;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt17;

internal class Art17GetAnimeDetailReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art17GetAnimeDetailRequest);

    public override Type FeatHandlerType => typeof(Art17GetAnimeDetailHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
