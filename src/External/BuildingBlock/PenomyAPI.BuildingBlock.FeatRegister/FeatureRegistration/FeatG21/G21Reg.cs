using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG21;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG21;

internal sealed class G21Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G21Request);

    public override Type FeatHandlerType => typeof(G21Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
