using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG2;

public sealed class G2Request : IFeatureRequest<G2Response>
{
    public ArtworkType ArtworkType { get; set; }
}
