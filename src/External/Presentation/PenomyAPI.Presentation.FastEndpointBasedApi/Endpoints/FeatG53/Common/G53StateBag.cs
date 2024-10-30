using PenomyAPI.App.FeatG53;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG53.Common;

internal sealed class G53StateBag
{
    internal G53Request AppRequest { get; } = new();
}
