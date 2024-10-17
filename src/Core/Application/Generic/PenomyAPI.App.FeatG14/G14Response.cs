using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG14;

public class G14Response : IFeatureResponse
{
    public List<ArtworkCategory> Result { get; set; }

    public G14ResponseStatusCode StatusCode { get; set; }
}
