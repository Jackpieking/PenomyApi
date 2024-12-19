using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG8.OtherHandlers;

public sealed class G8GetPaginationOptionsRequest
    : IFeatureRequest<G8GetPaginationOptionsResponse>
{
    public long ArtworkId { get; set; }
}
