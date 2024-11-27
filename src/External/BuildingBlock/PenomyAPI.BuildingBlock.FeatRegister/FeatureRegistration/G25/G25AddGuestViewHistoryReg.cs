using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25;

internal sealed class G25AddGuestViewHistoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25AddGuestViewHistoryRequest);

    public override Type FeatHandlerType => typeof(G25AddGuestViewHistoryHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
