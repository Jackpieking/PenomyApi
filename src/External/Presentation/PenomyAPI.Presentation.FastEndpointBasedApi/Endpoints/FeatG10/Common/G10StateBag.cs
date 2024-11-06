using PenomyAPI.App.FeatG10;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.Common;

internal sealed class G10StateBag
{
    internal G10Request AppRequest { get; } = new();
}
