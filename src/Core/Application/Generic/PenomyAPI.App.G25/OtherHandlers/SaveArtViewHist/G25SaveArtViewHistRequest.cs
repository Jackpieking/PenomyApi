using PenomyAPI.App.Common;

namespace PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;

public class G25SaveArtViewHistRequest : IFeatureRequest<G25SaveArtViewHistResponse>
{
    public long UserId { get; set; }

    public long ArtworkId { get; set; }

    public long ChapterId { get; set; }
}
