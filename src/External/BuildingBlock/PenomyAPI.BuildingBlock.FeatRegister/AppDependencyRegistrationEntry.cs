using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.Features;

namespace PenomyAPI.BuildingBlock.FeatRegister;

public static class AppDependencyRegistrationEntry
{
    public static void AddAppDependency(this IServiceCollection services)
    {
        FeatureHandlerDefinitionPreRegistration.Execute();
    }
}
