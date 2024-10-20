using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG21;

public class G21Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkComment> Result { get; set; }

    public G21ResponseStatusCode StatusCode { get; set; }
}
