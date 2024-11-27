using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM44;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM44;

internal sealed class SM44Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM44Request);

    public override Type FeatHandlerType => typeof(SM44Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
