using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt3.OtherHandlers.RemoveAllDeteledItems;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt3;

internal sealed class Art3RemoveAllDeletedItemsReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art3RemoveAllDeletedItemsRequest);

    public override Type FeatHandlerType => typeof(Art3RemoveAllDeletedItemsHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
