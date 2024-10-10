using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadCategory;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt7;

internal sealed class Art7LoadCategoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art7LoadCategoryRequest);

    public override Type FeatHandlerType => typeof(Art7LoadCategoryHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
