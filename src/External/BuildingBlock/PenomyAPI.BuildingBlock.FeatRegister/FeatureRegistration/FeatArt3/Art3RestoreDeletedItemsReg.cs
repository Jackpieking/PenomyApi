using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt3.OtherHandlers.RestoreDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt3;

internal class Art3RestoreDeletedItemsReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art3RestoreDeletedItemsRequest);

    public override Type FeatHandlerType => typeof(Art3RestoreDeletedItemsHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
