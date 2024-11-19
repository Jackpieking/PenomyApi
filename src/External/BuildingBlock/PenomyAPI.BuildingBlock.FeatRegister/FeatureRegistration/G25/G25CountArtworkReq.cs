using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25Reg;

internal sealed class G25CountArtworkReq : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25CountArtworkRequest);

    public override Type FeatHandlerType => typeof(G25CountArtworkHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
