using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM6;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM6;

internal sealed class SM6Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM6Request);

    public override Type FeatHandlerType => typeof(SM6Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
