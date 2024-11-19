using PenomyAPI.App.FeatG58;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG58.Common;

internal sealed class G58StateBag
{
    internal G58Request AppRequest { get; } = new();
}
