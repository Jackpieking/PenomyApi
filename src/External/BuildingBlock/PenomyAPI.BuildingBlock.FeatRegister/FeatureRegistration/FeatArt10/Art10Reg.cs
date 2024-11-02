using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt10;
using PenomyAPI.App.FeatArt10.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatArt10;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt10;

internal sealed class Art10Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art10Request);

    public override Type FeatHandlerType => typeof(Art10Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSingleton<IArt10FileService, Art10FileService>()
            .MakeSingletonLazy<IArt10FileService>();
    }
}
