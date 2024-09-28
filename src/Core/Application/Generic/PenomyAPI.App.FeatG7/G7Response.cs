using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG7;

public class G7Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<Artwork> Result { get; set; }

    public G7ResponseStatusCode StatusCode { get; set; }
}
