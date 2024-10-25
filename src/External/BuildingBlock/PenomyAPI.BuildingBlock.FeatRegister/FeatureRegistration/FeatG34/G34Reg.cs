using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG33;
using PenomyAPI.App.FeatG34;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG34;

internal sealed class G34Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G33Request);

    public override Type FeatHandlerType => typeof(G34Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
