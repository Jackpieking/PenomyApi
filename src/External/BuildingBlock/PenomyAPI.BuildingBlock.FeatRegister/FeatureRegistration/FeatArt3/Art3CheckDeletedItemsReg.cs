using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt3.OtherHandlers.CheckDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt3;

internal class Art3CheckDeletedItemsReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art3CheckDeletedItemsRequest);

    public override Type FeatHandlerType => typeof(Art3CheckDeletedItemsHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
