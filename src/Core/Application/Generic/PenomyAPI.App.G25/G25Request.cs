using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G25
{
    public class G25Request : IFeatureRequest<G25Response>
    {
        public long UserId { get; set; }
        public ArtworkType ArtworkType { get; set; }
        public int PageNum { get; set; } = 1;
        public int ArtNum { get; set; } = 20;
    }
}
