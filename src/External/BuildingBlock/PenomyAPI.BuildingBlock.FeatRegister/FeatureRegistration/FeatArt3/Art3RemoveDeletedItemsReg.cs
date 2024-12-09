using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt3.OtherHandlers.RemoveDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt3;

internal sealed class Art3RemoveDeletedItemsReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art3RemoveDeletedItemsRequest);

    public override Type FeatHandlerType => typeof(Art3RemoveDeletedItemsHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
