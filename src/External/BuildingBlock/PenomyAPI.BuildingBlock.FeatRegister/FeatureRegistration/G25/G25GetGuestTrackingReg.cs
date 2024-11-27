using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.GetGuestTracking;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25;

internal sealed class G25GetGuestTrackingReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25GetGuestTrackingRequest);

    public override Type FeatHandlerType => typeof(G25GetGuestTrackingHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
