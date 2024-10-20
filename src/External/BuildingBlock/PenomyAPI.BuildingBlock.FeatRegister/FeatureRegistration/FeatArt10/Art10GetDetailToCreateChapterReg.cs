using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt10.OtherHandlers.GetDetailToCreateChapter;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt10;

internal sealed class Art10GetDetailToCreateChapterReg
    : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art10GetDetailToCreateChapterRequest);

    public override Type FeatHandlerType => typeof(Art10GetDetailToCreateChapterHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
