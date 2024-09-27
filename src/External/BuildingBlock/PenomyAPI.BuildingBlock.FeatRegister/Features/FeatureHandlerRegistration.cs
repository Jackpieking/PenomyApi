using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.Features;

internal static class FeatureHandlerRegistration
{
    // Entry point.
    internal static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var registrationEntryAssembly = Assembly.GetAssembly(
            typeof(AppDependencyRegistrationEntry)
        );

        var featureDefinitionRegTypes = new List<Type>();

        foreach (var type in registrationEntryAssembly.GetTypes())
        {
            if (IsFeatureDefinitionRegistrationType(type))
            {
                featureDefinitionRegTypes.Add(type);
            }
        }

        if (featureDefinitionRegTypes.Count > 0)
        {
            RegisterFeature(featureDefinitionRegTypes, services, configuration);
        }
    }

    private static bool IsFeatureDefinitionRegistrationType(Type type)
    {
        // Type must be concrete class.
        if (type.IsAbstract)
        {
            return false;
        }

        return type.IsSubclassOf(typeof(FeatureDefinitionRegistration));
    }

    private static void RegisterFeature(
        List<Type> featureDefinitionRegTypes,
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        foreach (var type in featureDefinitionRegTypes)
        {
            var featureDefinitionRegistration = (IFeatureDefinitionRegistration)
                Activator.CreateInstance(type);

            // Add the feature definition to registry
            FeatureExtensions.FeatureHandlerRegistry.TryAdd(
                key: featureDefinitionRegistration.FeatRequestType,
                value: featureDefinitionRegistration.HandlerDefinition
            );

            // Add the feature dependency.
            featureDefinitionRegistration.AddFeatureDependency(services, configuration);
        }
    }
}
