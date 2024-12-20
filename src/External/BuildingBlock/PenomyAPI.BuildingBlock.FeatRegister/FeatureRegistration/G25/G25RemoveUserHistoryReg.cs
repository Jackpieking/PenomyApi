using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25;

internal sealed class G25RemoveUserHistoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G25RemoveUserHistoryItemRequest);

    public override Type FeatHandlerType => typeof(G25RemoveUserHistoryItemHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
