using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt3.OtherHandlers.RestoreAllDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt3;

internal class Art3RestoreAllDeletedItemsReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art3RestoreAllDeletedItemsRequest);

    public override Type FeatHandlerType => typeof(Art3RestoreAllDeletedItemsHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
