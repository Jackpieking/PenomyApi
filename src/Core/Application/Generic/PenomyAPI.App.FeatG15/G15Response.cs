using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG15;

public class G15Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public Artwork Result { get; set; }

    public G15ResponseStatusCode StatusCode { get; set; }
}
