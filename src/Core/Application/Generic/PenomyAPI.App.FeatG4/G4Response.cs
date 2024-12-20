using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;

namespace PenomyAPI.App.FeatG4;

public class G4Response : IFeatureResponse
{
    public List<ArtworkCategory> Result { get; set; }

    public List<RecommendedArtworkByCategory> RecommendedArtworkByCategories { get; set; }

    public G4ResponseStatusCode StatusCode { get; set; }
}
