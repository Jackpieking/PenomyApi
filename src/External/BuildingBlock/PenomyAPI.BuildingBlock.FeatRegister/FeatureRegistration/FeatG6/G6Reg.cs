using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.APP.FeatG6;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG6;

internal sealed class G6Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G6Request);

    public override Type FeatHandlerType => typeof(G6Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
