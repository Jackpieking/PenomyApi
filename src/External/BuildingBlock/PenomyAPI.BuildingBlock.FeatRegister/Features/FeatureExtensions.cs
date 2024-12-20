using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.BuildingBlock.FeatRegister.Common;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.Features;

public static class FeatureExtensions
{
    // Container is concurrent because of thread safe in multi-thread case.
    internal static readonly ConcurrentDictionary<
        Type,
        FeatureHandlerDefinition
    > FeatureHandlerRegistry = new();

    public static Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        TRequest request,
        CancellationToken ct
    )
        where TResponse : IFeatureResponse
        where TRequest : IFeatureRequest<TResponse>
    {
        var tRequest = request.GetType();

        // Try to get the feature handler definition from the container.
        FeatureHandlerRegistry.TryGetValue(tRequest, out var handlerDefinition);

        // Check if the handler definition is not null.
        if (Equals(handlerDefinition, default))
        {
            return Task.FromResult<TResponse>(default);
        }

        // Initialize target handler.
        var handler =
            FeatureHandlerResolver.CreateInstance(handlerDefinition.HandlerType)
            as IFeatureHandler<TRequest, TResponse>;

        // Execute the handler.
        return handler.ExecuteAsync(request, ct);
    }
}
