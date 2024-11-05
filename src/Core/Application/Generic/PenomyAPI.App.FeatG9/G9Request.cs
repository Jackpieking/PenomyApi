using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG9;

public sealed class G9Request : IFeatureRequest<G9Response>
{
    public long UserId { get; set; }

    public long ComicId { get; set; }

    public long ChapterId { get; set; }
}
