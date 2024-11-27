using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM42;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM42;

internal sealed class SM42Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM42Request);

    public override Type FeatHandlerType => typeof(SM42Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
