using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG4;

public class G4Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkCategory> Result { get; set; }

    public G4ResponseStatusCode StatusCode { get; set; }
}
