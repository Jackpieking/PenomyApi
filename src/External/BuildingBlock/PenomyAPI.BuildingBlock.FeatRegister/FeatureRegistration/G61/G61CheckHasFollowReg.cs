using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G61.OtherHandlers.CheckHasFollow;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G61;

internal sealed class G61CheckHasFollowReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G61CheckHasFollowRequest);

    public override Type FeatHandlerType => typeof(G61CheckHasFollowHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
