using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.GetGuestHistory;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25;

internal sealed class G25GetGuestViewHistoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25GetGuestHitstoryRequest);

    public override Type FeatHandlerType => typeof(G25GetGuestHistoryHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
