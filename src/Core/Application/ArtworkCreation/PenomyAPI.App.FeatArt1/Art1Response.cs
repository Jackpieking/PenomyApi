using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt1;

public sealed class Art1Response : IFeatureResponse
{
    public List<Artwork> Artworks { get; set; }

    public Art1ResponseAppCode AppCode { get; set; }
}
