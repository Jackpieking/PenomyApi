using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt12;

internal sealed class Art12ReloadChapterMediaReg
    : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art12ReloadChapterMediaRequest);

    public override Type FeatHandlerType => typeof(Art12ReloadChapterMediaHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
