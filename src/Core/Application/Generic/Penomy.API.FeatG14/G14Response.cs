using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG14;

public class G14Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<Artwork> Artworks { get; set; }

    public G14ResponseStatusCode StatusCode { get; set; }
}
