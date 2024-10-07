using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadPublicLevel;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt7;

internal sealed class Art7LoadPublicLevelReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art7LoadPublicLevelRequest);

    public override Type FeatHandlerType => typeof(Art7LoadPublicLevelHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
