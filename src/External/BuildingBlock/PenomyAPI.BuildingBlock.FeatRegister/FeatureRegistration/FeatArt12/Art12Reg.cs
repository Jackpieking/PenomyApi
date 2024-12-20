using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt12;
using PenomyAPI.App.FeatArt12.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatArt12;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt12;

internal sealed class Art12Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art12Request);

    public override Type FeatHandlerType => typeof(Art12Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSingleton<IArt12FileService, Art12FileService>()
            .MakeSingletonLazy<IArt12FileService>();
    }
}
