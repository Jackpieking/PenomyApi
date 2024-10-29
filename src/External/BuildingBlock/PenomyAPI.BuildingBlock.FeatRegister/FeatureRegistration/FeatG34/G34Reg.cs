using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG34;
using PenomyAPI.App.FeatG34.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatG34;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG34;

internal sealed class G34Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G34Request);

    public override Type FeatHandlerType => typeof(G34Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddSingleton<IG34PreResetPasswordTokenHandler, G34PreResetPasswordTokenHandler>()
            .MakeSingletonLazy<IG34PreResetPasswordTokenHandler>();
    }
}
