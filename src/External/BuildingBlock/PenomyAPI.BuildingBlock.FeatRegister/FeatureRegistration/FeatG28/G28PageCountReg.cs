using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG28.PageCount;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG28;

internal sealed class G28PageCountReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G28PageCountRequest);

    public override Type FeatHandlerType => typeof(G28PageCountHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
