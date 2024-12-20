using PenomyAPI.App.SM24;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM24.Common;

internal sealed class SM24StateBag
{
    internal SM24Request AppRequest { get; } = new();
}
