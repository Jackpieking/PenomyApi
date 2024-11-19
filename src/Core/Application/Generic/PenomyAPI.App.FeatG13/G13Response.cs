using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G13;

public class G13Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<Artwork> ArtworkList { get; set; }

    public G13ResponseStatusCode StatusCode { get; set; }
}
