using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.InitGuestHistory;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25;

internal sealed class G25InitGuestHistoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25InitGuestHistoryRequest);

    public override Type FeatHandlerType => typeof(G25InitGuestHistoryHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
