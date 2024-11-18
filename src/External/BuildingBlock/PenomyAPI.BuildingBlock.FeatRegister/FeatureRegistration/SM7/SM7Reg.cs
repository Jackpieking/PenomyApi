using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM7;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM7;

internal sealed class SM7Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM7Request);

    public override Type FeatHandlerType => typeof(SM7Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}