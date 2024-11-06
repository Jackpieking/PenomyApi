using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G25.OtherHandlers.NumberArtViewed;

public class G25CountArtViewedRequest : IFeatureRequest<G25CountArtViewedResponse>
{
    public long UserId { get; set; }
    public ArtworkType ArtworkType { get; set; }
}
