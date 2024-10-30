using PenomyAPI.App.FeatG59;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG59.Common;

internal sealed class G59StateBag
{
    internal G59Request AppRequest { get; } = new();
}
