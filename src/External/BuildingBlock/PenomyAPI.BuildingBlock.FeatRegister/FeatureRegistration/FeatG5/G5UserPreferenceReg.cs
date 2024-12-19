using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG5.OtherHandlers.UserPreferences;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG5;

internal class G5UserPreferenceReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G5UserPreferenceRequest);

    public override Type FeatHandlerType => typeof(G5UserPreferenceHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
