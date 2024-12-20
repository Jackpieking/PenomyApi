using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG20;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG20;

internal sealed class G20Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G20Request);

    public override Type FeatHandlerType => typeof(G20Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
