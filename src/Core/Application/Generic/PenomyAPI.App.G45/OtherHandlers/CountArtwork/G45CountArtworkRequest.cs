using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G45.OtherHandlers.CountArtwork
{
    public class G45CountArtworkRequest : IFeatureRequest<G45CountArtworkResponse>
    {
        public long UserId { get; set; }
        public ArtworkType ArtworkType { get; set; }
    }
}
