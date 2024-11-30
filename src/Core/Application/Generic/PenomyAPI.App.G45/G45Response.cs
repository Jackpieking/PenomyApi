using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG45;
using System.Collections.Generic;

namespace PenomyAPI.App.G45;

public class G45Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public List<G45FollowedArtworkReadModel> Result { get; set; }
    public G45ResponseStatusCode StatusCode { get; set; }
}
