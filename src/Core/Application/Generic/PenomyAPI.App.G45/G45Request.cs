using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.G45;

public class G45Request : IFeatureRequest<G45Response>
{
    public long UserId { get; set; }
    public ArtworkType ArtworkType { get; set; }
    public int PageNum { get; set; } = 1;
    public int ArtNum { get; set; } = 20;
}
