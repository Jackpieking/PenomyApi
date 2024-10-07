using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G25
{
    public class G25SaveArtViewHistRequest : IFeatureRequest<G25SaveArtViewHistResponse>
    {
        public long UserId { get; set; }
        public long ArtworkId { get; set; }
        public long ChapterId { get; set; }
        public ArtworkType ArtworkType { get; set; }
    }
}
