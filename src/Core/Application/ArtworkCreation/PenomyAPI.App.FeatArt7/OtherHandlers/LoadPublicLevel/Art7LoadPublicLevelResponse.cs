using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadPublicLevel;

public sealed class Art7LoadPublicLevelResponse : IFeatureResponse
{
    public ArtworkPublicLevel[] PublicLevels { get; init; }
}
