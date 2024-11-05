using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G48.OtherHandlers.CountArtwork
{
    public class G48CountArtworkRequest : IFeatureRequest<G48CountArtworkResponse>
    {
        public long UserId { get; set; }
        public ArtworkType ArtworkType { get; set; }
    }
}
