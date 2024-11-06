using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG28;

public class G28Response : IFeatureResponse
{
    public List<Artwork> result { get; set; }

    public G28ResponseStatusCode StatusCode { get; set; }
}
