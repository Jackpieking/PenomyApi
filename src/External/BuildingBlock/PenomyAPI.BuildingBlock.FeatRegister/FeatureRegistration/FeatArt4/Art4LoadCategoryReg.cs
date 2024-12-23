﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt1;

internal sealed class Art4LoadCategoryReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art4LoadCategoryRequest);

    public override Type FeatHandlerType => typeof(Art4LoadCategoryHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
