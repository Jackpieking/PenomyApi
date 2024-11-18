using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM7.OtherHandlers.CountGroups;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM7;

internal sealed class SM7CountGroupsReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM7CountGroupsRequest);

    public override Type FeatHandlerType => typeof(SM7CountGroupsHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}