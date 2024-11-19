using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt12;

internal sealed class Art12GetChapterDetailReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art12GetChapterDetailRequest);

    public override Type FeatHandlerType => typeof(Art12GetChapterDetailHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
