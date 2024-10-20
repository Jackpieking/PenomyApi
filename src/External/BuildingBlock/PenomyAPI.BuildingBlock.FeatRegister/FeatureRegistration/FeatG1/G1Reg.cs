using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG1;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatG1;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG1;

internal sealed class G1Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G1Request);

    public override Type FeatHandlerType => typeof(G1Handler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IG1PreRegistrationTokenHandler, G1PreRegistrationTokenHandler>()
            .MakeSingletonLazy<IG1PreRegistrationTokenHandler>();
    }
}
