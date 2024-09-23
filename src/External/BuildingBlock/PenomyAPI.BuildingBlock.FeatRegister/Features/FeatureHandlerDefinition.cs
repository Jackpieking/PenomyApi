using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.Features;

internal sealed class FeatureHandlerDefinition
{
    internal Type HandlerType { get; set; }

    internal FeatureHandlerDefinition(Type handlerType)
    {
        HandlerType = handlerType;
    }
}
