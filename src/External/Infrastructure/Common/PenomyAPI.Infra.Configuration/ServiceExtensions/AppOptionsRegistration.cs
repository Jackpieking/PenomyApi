using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.Infra.Configuration.Common;

namespace PenomyAPI.Infra.Configuration.ServiceExtensions;

public static class AppOptionsRegistration
{
    /// <summary>
    ///     Register all the <see cref="AppOption"/> that defined
    ///     in the application using reflection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection Register(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Get the assembly contain the type AppOptions to begin reflection.
        var appOptionsAssembly = Assembly.GetAssembly(type: typeof(AppOptions));

        var appOptionsTypes = new List<Type>();

        foreach (var type in appOptionsAssembly.GetTypes())
        {
            var isAppOptions = type.IsSubclassOf(typeof(AppOptions));

            if (isAppOptions)
            {
                appOptionsTypes.Add(type);
            }
        }

        // If list of AppOption types is empty, then skip the services collection injection.
        if (!appOptionsTypes.Any())
        {
            return services;
        }

        // Add the app options into services collection.
        foreach (var appOptionType in appOptionsTypes)
        {
            var appOption = Activator.CreateInstance(appOptionType) as AppOptions;

            // Bind the app option from given configuration.
            appOption.Bind(configuration);

            services.AddSingleton(appOptionType, appOption);
        }

        return services;
    }
}
