using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG10;

public class G10Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkComment> Result { get; set; }

    public G10ResponseStatusCode StatusCode { get; set; }
}
