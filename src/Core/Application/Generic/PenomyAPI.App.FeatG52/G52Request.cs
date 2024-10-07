using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG52;

public class G52Request : IFeatureRequest<G52Response>
{
    public ArtworkComment ArtworkComment { get; init; }

}
