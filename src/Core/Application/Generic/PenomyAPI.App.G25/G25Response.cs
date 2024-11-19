using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G25;

public class G25Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<IEnumerable<UserArtworkViewHistory>> Result { get; set; }
    public G25ResponseStatusCode StatusCode { get; set; }
}
