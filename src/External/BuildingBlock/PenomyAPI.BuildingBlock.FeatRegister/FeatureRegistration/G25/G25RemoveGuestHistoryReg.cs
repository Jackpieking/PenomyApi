using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.RemoveGuestHistoryItem;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25;

internal sealed class G25RemoveGuestHistoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25RemoveGuestHistoryItemRequest);

    public override Type FeatHandlerType => typeof(G25RemoveGuestHistoryItemHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
