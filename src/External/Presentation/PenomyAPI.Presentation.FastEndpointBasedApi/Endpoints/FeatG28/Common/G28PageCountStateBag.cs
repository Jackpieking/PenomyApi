using PenomyAPI.App.FeatG28.PageCount;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Common;

internal sealed class G28PageCountStateBag
{
    internal G28PageCountRequest PageCountRequest { get; } = new();
}
