using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM34;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM34;

internal sealed class SM34Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM34Request);

    public override Type FeatHandlerType => typeof(SM34Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
