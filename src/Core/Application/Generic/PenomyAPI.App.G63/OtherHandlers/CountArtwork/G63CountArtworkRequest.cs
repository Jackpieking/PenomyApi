using PenomyAPI.App.Common;

namespace PenomyAPI.App.G63.OtherHandlers.CountArtwork
{
    public class G63CountArtworkRequest : IFeatureRequest<G63CountArtworkResponse>
    {
        public long UserId { get; set; }
    }
}
