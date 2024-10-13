using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.APP.FeatG6;

public class G6Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<Artwork> Result { get; set; }

    public G6ResponseStatusCode StatusCode { get; set; }
}
