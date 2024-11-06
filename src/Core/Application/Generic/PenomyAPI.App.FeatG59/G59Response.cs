using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG59;

public class G59Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkComment> Result { get; set; }

    public G59ResponseStatusCode StatusCode { get; set; }
}
