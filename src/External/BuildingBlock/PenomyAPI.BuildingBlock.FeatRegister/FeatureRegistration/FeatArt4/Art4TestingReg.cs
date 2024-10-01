using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt4.OtherHandlers.TestingSnowflake;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt4;

internal sealed class Art4TestingReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art4TestingRequest);

    public override Type FeatHandlerType => typeof(Art4TestingHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
