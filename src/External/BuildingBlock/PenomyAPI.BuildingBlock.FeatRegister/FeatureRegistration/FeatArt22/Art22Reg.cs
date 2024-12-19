using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt22;
using PenomyAPI.App.FeatArt22.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatArt20;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt22;

internal class Art22Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art22Request);

    public override Type FeatHandlerType => typeof(Art22Handler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IArt22FileService, Art20FileService>()
            .MakeScopedLazy<IArt22FileService>();
    }
}
