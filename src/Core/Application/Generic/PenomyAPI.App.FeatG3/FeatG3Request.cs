using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Request : IFeatureRequest<FeatG3Response>
{
    public ArtworkType ArtworkType { get; set; }
}
