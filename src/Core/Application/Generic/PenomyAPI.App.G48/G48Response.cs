using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG48;
using System.Collections.Generic;

namespace PenomyAPI.App.G48;

public class G48Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public List<G48FavoriteArtworkReadModel> Result { get; set; }
    public G48ResponseStatusCode StatusCode { get; set; }
}
