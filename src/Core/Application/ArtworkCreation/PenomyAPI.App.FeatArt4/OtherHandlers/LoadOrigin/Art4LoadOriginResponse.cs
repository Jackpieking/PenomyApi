using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadOrigin;

public sealed class Art4LoadOriginResponse : IFeatureResponse
{
    public IEnumerable<ArtworkOrigin> Origins { get; init; }
}
