using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G48;

public class G48Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }
    public ICollection<Artwork> Result { get; set; }
    public G48ResponseStatusCode StatusCode { get; set; }
}
