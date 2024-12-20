using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG49.OtherHandlers;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G49;

internal sealed class G49OtherReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G49StarRateRequest);

    public override Type FeatHandlerType => typeof(G49StarRateHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
