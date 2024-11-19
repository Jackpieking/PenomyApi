using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.G45;

public class G45Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<Artwork> Result { get; set; }
    public G45ResponseStatusCode StatusCode { get; set; }
}
