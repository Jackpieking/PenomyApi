using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt20.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt20;

internal class Art20GetDetailToCreateChapReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art20GetDetailToCreateChapRequest);

    public override Type FeatHandlerType => typeof(Art20GetDetailToCreateChapHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
