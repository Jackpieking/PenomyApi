﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.App.FeatArt4.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatArt4;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt4;

// Phải extends lại abstract-class FeatureDefinitionRegistration
internal sealed class Art4Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art4Request); // IFeatureRequest của feat

    public override Type FeatHandlerType => typeof(Art4Handler); // IFeatureResponse của feat

    // Override lại hàm này để đăng ký các dependencies cần thiết cho feat-handler.
    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IFeatArt4FileUploadService, FeatArt4FileUploadService>()
            .MakeScopedLazy<IFeatArt4FileUploadService>();
    }
}