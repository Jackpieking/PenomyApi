using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G13;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G13;

internal sealed class G13Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G13Request);

    public override Type FeatHandlerType => typeof(G13Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
