using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG19.OtherHandlers.GetChapterList;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G19;

internal class G19GetChapterListReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G19GetChapterListRequest);

    public override Type FeatHandlerType => typeof(G19GetChapterListHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
