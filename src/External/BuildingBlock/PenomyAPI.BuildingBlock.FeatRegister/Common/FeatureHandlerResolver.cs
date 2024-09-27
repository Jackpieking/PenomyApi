using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace PenomyAPI.BuildingBlock.FeatRegister.Common;

public static class FeatureHandlerResolver
{
    private static IServiceProvider _provider;

    private static readonly ConcurrentDictionary<Type, ObjectFactory> _factoryCache = new();

    public static void SetProvider(IServiceProvider value)
    {
        // Check if the provider is already set.
        if (!Equals(_provider, default))
        {
            return;
        }

        // Set the provider.
        _provider = value.CreateScope().ServiceProvider;
    }

    internal static object CreateInstance(Type type)
    {
        // Get the factory from the cache.
        var factory = _factoryCache.GetOrAdd(type, FactoryInitializer);

        // Execute the factory.
        return factory(_provider, null);

        // Initialize the factory.
        static ObjectFactory FactoryInitializer(Type t)
        {
            return ActivatorUtilities.CreateFactory(t, Type.EmptyTypes);
        }
    }
}
