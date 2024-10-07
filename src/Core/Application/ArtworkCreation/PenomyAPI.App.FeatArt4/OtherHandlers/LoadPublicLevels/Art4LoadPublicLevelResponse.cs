using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadPublicLevels;

public sealed class Art4LoadPublicLevelResponse : IFeatureResponse
{
    public ArtworkPublicLevel[] PublicLevels { get; init; }
}
