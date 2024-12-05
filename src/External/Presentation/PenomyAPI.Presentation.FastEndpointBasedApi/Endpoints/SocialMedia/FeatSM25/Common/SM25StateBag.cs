using PenomyAPI.App.SM25;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM25.Common;

internal sealed class SM25StateBag
{
    internal SM25Request AppRequest { get; } = new();
}
