using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG20;

public class G20Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkComment> Result { get; set; }

    public G20ResponseStatusCode StatusCode { get; set; }
}
