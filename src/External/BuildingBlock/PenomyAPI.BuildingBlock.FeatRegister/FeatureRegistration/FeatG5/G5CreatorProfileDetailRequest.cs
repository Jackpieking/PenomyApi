using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG5.OtherHandlers.CreatorProfileDetail;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG5;

internal class G5CreatorProfileDetailRequest : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G5CreatorProfileDetailRequest);

    public override Type FeatHandlerType => typeof(G5CreatorProfileDetailHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
