using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt7;

internal sealed class Art7LoadComicDetailReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art7LoadComicDetailRequest);

    public override Type FeatHandlerType => typeof(Art7LoadComicDetailHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
