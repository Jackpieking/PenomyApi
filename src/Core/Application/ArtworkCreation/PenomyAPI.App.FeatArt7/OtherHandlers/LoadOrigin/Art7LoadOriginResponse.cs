using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadOrigin;

public sealed class Art7LoadOriginResponse : IFeatureResponse
{
    public IEnumerable<ArtworkOrigin> Origins { get; init; }
}
