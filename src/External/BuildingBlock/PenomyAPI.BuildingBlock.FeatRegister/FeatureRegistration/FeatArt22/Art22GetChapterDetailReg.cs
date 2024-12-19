using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt22;

internal class Art22GetChapterDetailReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art22GetChapterDetailRequest);

    public override Type FeatHandlerType => typeof(Art22GetChapterDetailHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
