using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt20;
using PenomyAPI.App.FeatArt20.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatArt20;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt20;

internal class Art20Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art20Request);

    public override Type FeatHandlerType => typeof(Art20Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<IArt20FileService, Art20FileService>()
            .MakeScopedLazy<IArt20FileService>();
    }
}
