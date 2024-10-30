using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG31A;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG31A;

internal sealed class G31AReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G31ARequest);

    public override Type FeatHandlerType => typeof(G31AHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
