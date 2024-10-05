using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG5;

public class G5Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public Artwork Result { get; set; }

    public G5ResponseStatusCode StatusCode { get; set; }
}
