using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG12;

public class G12Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkCategory> Result { get; set; }

    public G12ResponseStatusCode StatusCode { get; set; }
}
