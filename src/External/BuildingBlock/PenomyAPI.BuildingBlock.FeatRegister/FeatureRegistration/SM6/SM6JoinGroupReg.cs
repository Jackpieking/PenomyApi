using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM6.OtherHandlers.JoinGroup;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM6;

internal sealed class SM6JoinGroupReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM6JoinGroupRequest);

    public override Type FeatHandlerType => typeof(SM6JoinGroupHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
