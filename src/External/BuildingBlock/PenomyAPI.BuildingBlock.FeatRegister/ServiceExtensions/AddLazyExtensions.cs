using Microsoft.Extensions.DependencyInjection;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions
{
    internal static class AddLazyExtensions
    {
        public static IServiceCollection MakeSingletonLazy<T>(this IServiceCollection services)
            where T : class
        {
            return services.AddSingleton<Lazy<T>>(implementationFactory: provider =>
                new(valueFactory: () => provider.GetRequiredService<T>())
            );
        }

        public static IServiceCollection MakeScopedLazy<T>(this IServiceCollection services)
            where T : class
        {
            return services.AddScoped<Lazy<T>>(implementationFactory: provider =>
                new(valueFactory: () => provider.GetRequiredService<T>())
            );
        }
    }
}
