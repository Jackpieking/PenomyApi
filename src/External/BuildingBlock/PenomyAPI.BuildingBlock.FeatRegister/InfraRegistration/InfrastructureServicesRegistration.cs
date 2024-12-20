using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration;

internal static class InfrastructureServicesRegistration
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var registrationEntryAssembly = Assembly.GetAssembly(
            typeof(AppDependencyRegistrationEntry)
        );

        var servicesRegistrationTypes = new List<Type>();

        // Get all types that inherit from FeatureDefinitionRegistration.
        foreach (var type in registrationEntryAssembly.GetTypes())
        {
            if (IsServicesRegistrationType(type))
            {
                servicesRegistrationTypes.Add(type);
            }
        }

        // If the list is empty, then skip the services registration.
        if (servicesRegistrationTypes.Count < 0)
        {
            return;
        }

        // Register the services.
        foreach (var type in servicesRegistrationTypes)
        {
            var servicesRegistration = Activator.CreateInstance(type) as IServiceRegistration;

            servicesRegistration.Register(services, configuration);
        }
    }

    /// <summary>
    ///     Check if the input type is type of <see cref="IServicesRegistration"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsServicesRegistrationType(Type type)
    {
        var typeInheritInterfaces = type.GetInterfaces();

        if (!typeInheritInterfaces.Any())
        {
            return false;
        }

        var interfaceType = typeInheritInterfaces.First();

        return interfaceType.AssemblyQualifiedName.Equals(
            value: typeof(IServiceRegistration).AssemblyQualifiedName
        );
    }
}
