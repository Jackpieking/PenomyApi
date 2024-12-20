using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG5.OtherHandlers.GetArtworkMetaData;

public class G5ArtworkMetaDataRequest : IFeatureRequest<G5ArtworkMetaDataResponse>
{
    public long ArtworkId { get; set; }
}
