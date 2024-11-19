using PenomyAPI.App.FeatG28;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Common;

internal sealed class G28StateBag
{
    internal G28Request AppRequest { get; } = new();
}
