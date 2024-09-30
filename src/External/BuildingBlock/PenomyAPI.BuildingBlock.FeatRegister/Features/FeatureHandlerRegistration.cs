using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.Common;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PenomyAPI.BuildingBlock.FeatRegister.Features;

internal static class FeatureHandlerRegistration
{
    /// <summary>
    ///     Register all <see cref="FeatureDefinitionRegistration"/> defined in the application.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    internal static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var registrationEntryAssembly = Assembly.GetAssembly(
            typeof(AppDependencyRegistrationEntry)
        );

        var featureDefinitionRegTypes = new List<Type>();

        // Get all types that inherit from FeatureDefinitionRegistration.
        foreach (var type in registrationEntryAssembly.GetTypes())
        {
            if (IsFeatureDefinitionRegistrationType(type))
            {
                featureDefinitionRegTypes.Add(type);
            }
        }

        // If the list is empty, then skip the feature dependencies registration.
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

            if (!featureDefinitionRegistration.IsRequestAndHanlderMatched())
            {
                throw new FeatureRegistrationException(
                    $"The registered requestType is [{featureDefinitionRegistration.FeatRequestType.FullName}] but the provided handlerType is [{featureDefinitionRegistration.FeatHandlerType.FullName}]");
            }

            // Add the feature definition to registry.
            FeatureExtensions.FeatureHandlerRegistry.TryAdd(
                key: featureDefinitionRegistration.FeatRequestType,
                value: featureDefinitionRegistration.HandlerDefinition
            );

            // Add the feature dependency.
            featureDefinitionRegistration.AddFeatureDependency(services, configuration);
        }
    }
}
