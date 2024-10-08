using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

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
        _provider = value;
    }

    internal static object CreateInstance(Type type)
    {
        // Get the factory from the cache.
        var factory = _factoryCache.GetOrAdd(type, FactoryInitializer);

        // Create an scope service provider.
        var scopeServiceProvider = _provider.CreateScope().ServiceProvider;

        // Execute the factory.
        return factory(scopeServiceProvider, null);

        // Initialize the factory.
        static ObjectFactory FactoryInitializer(Type t)
        {
            return ActivatorUtilities.CreateFactory(t, Type.EmptyTypes);
        }
    }
}
