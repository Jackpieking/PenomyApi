using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG19;

public class G19Request : IFeatureRequest<G19Response>
{
    public long UserId { get; set; }

    public long AnimeId { get; set; }

    public long ChapterId { get; set; }
}
