using PenomyAPI.App.SM8;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.Common;

internal sealed class SM8StateBag
{
    internal SM8Request AppRequest { get; } = new();
}
