using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Response : IFeatureResponse
{
    public IEnumerable<Artwork> Artworks { get; set; }

    public FeatG3ResponseStatusCode StatusCode { get; set; }
}
