using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG9.OtherHandlers.GetChapterList;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG9;

internal sealed class G9GetChapterListReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G9GetChapterListRequest);
    public override Type FeatHandlerType => typeof(G9GetChapterListHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
