using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Sys1;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM44;

internal sealed class Sys1Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Sys1Request);

    public override Type FeatHandlerType => typeof(Sys1Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
