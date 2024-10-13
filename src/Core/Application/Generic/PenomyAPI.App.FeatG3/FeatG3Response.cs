using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<Artwork> ArtworkList { get; set; }

    public FeatG3ResponseStatusCode StatusCode { get; set; }
}
