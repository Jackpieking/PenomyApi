using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G48;

public class G48Request : IFeatureRequest<G48Response>
{
    public long UserId { get; set; }
    public ArtworkType ArtworkType { get; set; }
    public int PageNum { get; set; }
    public int ArtNum { get; set; }
}
