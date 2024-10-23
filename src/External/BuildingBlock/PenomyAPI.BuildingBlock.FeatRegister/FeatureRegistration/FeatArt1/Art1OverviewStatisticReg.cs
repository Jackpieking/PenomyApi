using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt1.OtherHandlers.OverviewStatistic;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt1;

internal sealed class Art1OverviewStatisticReg
    : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art1OverviewStatisticRequest);

    public override Type FeatHandlerType => typeof(Art1OverviewStatisticHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
