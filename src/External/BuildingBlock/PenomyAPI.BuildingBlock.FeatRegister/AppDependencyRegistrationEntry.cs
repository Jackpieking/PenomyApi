using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration.Handler;
using PenomyAPI.Infra.Configuration.ServiceExtensions;

namespace PenomyAPI.BuildingBlock.FeatRegister;

public static class AppDependencyRegistrationEntry
{
    public static void AddAppDependency(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        FeatureHandlerRegistration.Register(services, configuration);

        AppOptionsRegistration.Register(services, configuration);

        InfrastructureServicesRegistration.Register(services, configuration);
    }
}
