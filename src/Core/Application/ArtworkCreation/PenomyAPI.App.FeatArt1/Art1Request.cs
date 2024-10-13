using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt1;

public sealed class Art1Request : IFeatureRequest<Art1Response>
{
    public long CreatorId { get; set; }

    public ArtworkType ArtworkType { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}
