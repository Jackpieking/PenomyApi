using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG59;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG59;

internal sealed class G59Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G59Request);

    public override Type FeatHandlerType => typeof(G59Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
