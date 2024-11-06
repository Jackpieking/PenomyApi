using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG4;

public class G4Response : IFeatureResponse
{
    public List<ArtworkCategory> Result { get; set; }

    public G4ResponseStatusCode StatusCode { get; set; }
}
