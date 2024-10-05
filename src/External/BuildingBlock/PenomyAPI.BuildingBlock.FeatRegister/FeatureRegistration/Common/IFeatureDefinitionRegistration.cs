using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

/// <summary>
///     Based interface for any feature registration to inherit from.
///     This interface contains information that supported reflection
///     to inject the dependencies of the related feature.
/// </summary>
internal interface IFeatureDefinitionRegistration
{
    /// <summary>
    ///     The type of FeatureRequest that the FeatureHandler will handle.
    /// </summary>
    public Type FeatRequestType { get; }

    /// <summary>
    ///     The type of FeatureHandler that will be registered.
    /// </summary>
    public Type FeatHandlerType { get; }

    public FeatureHandlerDefinition HandlerDefinition { get; }

    /// <summary>
    ///     Check if the FeatRequestType and FeatHandlerType is matched
    ///     and suitable to register into FeatureHandlerRegistry.
    /// </summary>
    bool IsRequestAndHanlderMatched()
    {
        var featureHandlerInterfaceType = FeatHandlerType.GetInterfaces().FirstOrDefault();

        var handlerTypeGenericArguments = featureHandlerInterfaceType.GetGenericArguments();

        if (!handlerTypeGenericArguments.Any())
        {
            return false;
        }

        // The first generic argument is the request type of the handler.
        var handlerRequestType = handlerTypeGenericArguments[0];

        // Check if the registered featureDefinition is correct or not.
        return handlerRequestType.Name.Equals(FeatRequestType.Name);
    }

    /// <summary>
    ///     Register any feature dependencies in this function.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    void AddFeatureDependency(IServiceCollection services, IConfiguration configuration);
}
